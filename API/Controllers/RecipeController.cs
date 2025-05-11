using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class RecipeController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;
    public RecipeController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }
    [HttpPost]
    public async Task<ActionResult<RecipeDTO>> CreateRecipe([FromBody] RecipeDTO recipe)
    {
        var recipeServiceResult = await _unitOfServices.RecipeService.Create(recipe);
        return recipeServiceResult;
    }
    [HttpGet]
    public async Task<ActionResult<ICollection<RecipeDTO>>> GetRecipes()
    {
        var recipesServiceResult = await _unitOfServices.RecipeService.GetAllAsync();
        return recipesServiceResult;
    }
}