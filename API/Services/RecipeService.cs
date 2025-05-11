using System;
using API.Controllers;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class RecipeService : BaseService<Recipe, RecipeDTO>, IRecipeService
{
    public RecipeService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public override async Task<ActionResult<RecipeDTO>> Create(RecipeDTO recipe)
    {
        var recipeEntity = _mapper.Map<Recipe>(recipe);
        await _unitOfWork.RecipeRepository.AddAsync(recipeEntity);
        var result = await _unitOfWork.SaveAsync();
        if(result == false)
        {
            return new BadRequestObjectResult("Failed to create recipe");
        }
        return _mapper.Map<RecipeDTO>(recipeEntity);
    }

}
