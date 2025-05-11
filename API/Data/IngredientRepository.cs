using System;
using API.Entities;
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
}
