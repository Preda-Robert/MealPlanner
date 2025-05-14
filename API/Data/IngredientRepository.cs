using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(DataContext context) : base(context)
    {
    }

    public async Task<Ingredient?> GetIngredientByNameAsync(string name)
    {
        return await _context.Ingredients
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Name == name);
    }

    public Task<PagedList<Ingredient>> GetIngredientsAsync(IngredientParams ingredientParams)
    {
        var query = _context.Ingredients.AsQueryable();

        if (!string.IsNullOrEmpty(ingredientParams.SearchTerm))
        {
            query = query.Where(i => i.Name.ToLower().Contains(ingredientParams.SearchTerm.ToLower()));
        }

        return PagedList<Ingredient>.CreateAsync(query, ingredientParams.PageNumber, ingredientParams.PageSize);
    }
}
