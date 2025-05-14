using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public Task<ICollection<Recipe>?> GetRecipesByUserIdAsync(string userId)
    {
        return null!;
    }

    public async Task<Recipe?> GetRecipeByNameAsync(string name)
    {
        return await _context.Recipes.FirstOrDefaultAsync(r => r.Name == name);
    }

    public Task<PagedList<Recipe>> GetRecipesByAllergyIdAsync(RecipeParams recipeParams)
    {
        var query = _context.Recipes.AsQueryable();

        if (!string.IsNullOrEmpty(recipeParams.SearchTerm))
        {
            query = query.Where(a => a.Name.ToLower().Contains(recipeParams.SearchTerm.ToLower()));
        }

        foreach (var allergyId in recipeParams.AllergyIds)
        {
            query = query.Where(a => a.Id != allergyId);
        }

        return PagedList<Recipe>.CreateAsync(query, recipeParams.PageNumber, recipeParams.PageSize);
    }
}
