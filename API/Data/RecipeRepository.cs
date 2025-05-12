using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;

namespace API.Data;

public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
{
    public RecipeRepository(DataContext context) : base(context)
    {
    }

    public async Task<Recipe?> GetRecipeByIdAsync(int id)
    {
        return await _context.Recipes.FindAsync(id);
    }

    public Task<ICollection<Recipe>?> GetRecipesAsync(RecipeParams recipeParams)
    {
        return null!;
    }
}
