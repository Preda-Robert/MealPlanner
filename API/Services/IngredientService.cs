using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public class IngredientService : BaseService<Ingredient, IngredientDTO>, IIngredientService
{
    public IngredientService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public override async Task<ActionResult<IngredientDTO>> Create(IngredientDTO dto)
    {

        var ingredient = await _unitOfWork.IngredientRepository.GetIngredientByNameAsync(dto.Name);
        if(ingredient != null)
        {
            return _mapper.Map<IngredientDTO>(ingredient);
        }

        var category = await _unitOfWork.IngredientCategoryRepository.GetCategoryByNameAsync(dto.Category.Name);

        if (category == null)
        {
            return new BadRequestObjectResult($"Category with name {dto.Category.Name} does not exist.");
        }

        var createdIngredient = _mapper.Map<Ingredient>(dto);
        createdIngredient.Category = category;
        await _unitOfWork.IngredientRepository.AddAsync(createdIngredient);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<IngredientDTO>(createdIngredient);
    }

}
