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
    private readonly IIngredientService _ingredientService;
    private readonly ICookwareService _cookwareService;
    public RecipeService(IIngredientService ingredientService, ICookwareService cookwareService, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _ingredientService = ingredientService;
        _cookwareService = cookwareService;
    }

    public override async Task<ActionResult<RecipeDTO>> Create(RecipeDTO recipe)
    {
        string logPath = "logs/recipe_seeding.log";
        Directory.CreateDirectory("logs");

        void Log(string message)
        {
            File.AppendAllText(logPath, $"[{DateTime.Now}] {message}{Environment.NewLine}");
        }

        try
        {
            Log($"Attempting to create recipe: {recipe.Name}");

            Recipe? existingRecipe = await _unitOfWork.RecipeRepository.GetRecipeByNameAsync(recipe.Name);
            if (existingRecipe != null)
            {
                Log($"Recipe '{recipe.Name}' already exists. Skipping.");
                return new BadRequestObjectResult($"Recipe with name {recipe.Name} already exists.");
            }

            var recipeEntity = new Recipe
            {
                Name = recipe.Name,
                Description = recipe.Description
            };

            recipeEntity.ServingType = _mapper.Map<ServingType>(recipe.ServingType);
            recipeEntity.Difficulty = recipe.Difficulty;
            recipeEntity.CookingTime = recipe.CookingTime;

            await _unitOfWork.RecipeRepository.AddAsync(recipeEntity);
            await _unitOfWork.SaveAsync();
            Log($"Created base recipe entity: {recipe.Name}");

            foreach (var ingredientRecipe in recipe.Ingredients)
            {
                var ingredient = ingredientRecipe.Ingredient;
                Log($"Processing ingredient: {ingredient.Name}");

                var ingredientResult = await _ingredientService.Create(ingredient);
                if (ingredientResult.Result == null)
                {
                    Log($"Failed to create ingredient: {ingredient.Name}");
                    return new BadRequestObjectResult($"Ingredient with name {ingredient.Name} does not exist.");
                }

                var ingredientEntity = _mapper.Map<Ingredient>(ingredientResult.Result);
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = ingredientEntity,
                    Quantity = ingredientRecipe.Quantity,
                    IngredientId = ingredientEntity.Id,
                    RecipeId = recipeEntity.Id
                };
                recipeEntity.Ingredients.Add(recipeIngredient);
                Log($"Added ingredient to recipe: {ingredient.Name}");
            }

            foreach (var cookwareRecipe in recipe.Cookware)
            {
                Log($"Processing cookware: {cookwareRecipe.Name}");

                var cookwareResult = await _cookwareService.Create(cookwareRecipe);
                if (cookwareResult.Result == null)
                {
                    Log($"Failed to create cookware: {cookwareRecipe.Name}");
                    return new BadRequestObjectResult($"Cookware with name {cookwareRecipe.Name} does not exist.");
                }

                var cookwareEntity = _mapper.Map<Cookware>(cookwareResult);
                var recipeCookware = new RecipeCookware
                {
                    Cookware = cookwareEntity,
                    RecipeId = recipeEntity.Id,
                    CookwareId = cookwareEntity.Id
                };
                recipeEntity.Cookware.Add(recipeCookware);
                Log($"Added cookware to recipe: {cookwareRecipe.Name}");
            }

            foreach (var instruction in recipe.Instructions)
            {
                var recipeInstruction = new RecipeInstruction
                {
                    Description = instruction.Description,
                    InstructionIndex = instruction.InstructionIndex,
                    RecipeId = recipeEntity.Id
                };
                recipeEntity.Instructions.Add(recipeInstruction);
                Log($"Added instruction step {instruction.InstructionIndex} to recipe: {recipe.Name}");
            }

            Log($"Finalizing recipe: {recipe.Name}");
            await _unitOfWork.SaveAsync();

            Log($"Successfully created recipe: {recipe.Name}");

            var recipeDTO = _mapper.Map<RecipeDTO>(recipeEntity);
            return recipeDTO;
        }
        catch (Exception ex)
        {
            Log($"Exception while creating recipe '{recipe.Name}': {ex.Message}");
            throw;
        }
    }


    public async Task<ActionResult<RecipeDTO>> CreateUserRecipe(string username, RecipeDTO recipe)
    {
        return new OkObjectResult(recipe);
    }

}
