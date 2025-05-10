using System;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class IngredientController : BaseAPIController
{
    private readonly IUnitOfServices _unitOfServices;
    public IngredientController(IUnitOfServices unitOfServices)
    {
        _unitOfServices = unitOfServices;
    }
    [HttpPost]
    public async Task<ActionResult<IngredientDTO>> CreateIngredient([FromBody] IngredientDTO ingredient)
    {
        var ingredientServiceResult = await _unitOfServices.IngredientService.Create(ingredient);
        return ingredientServiceResult;
    }
    [HttpGet]
    public async Task<ActionResult<ICollection<IngredientDTO>>> GetIngredients()
    {
        var ingredientsServiceResult = await _unitOfServices.IngredientService.GetAllAsync();
        return ingredientsServiceResult;
    }
}
