using System;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IRecipeRepository : IBaseRepository<Recipe>
{
    Task<Recipe?> GetRecipeByIdAsync(int id);
    Task<ICollection<Recipe>?> GetRecipesAsync(RecipeParams recipeParams);
}
