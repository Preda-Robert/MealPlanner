using System;
using API.Entities;

namespace API.Interfaces;

public interface IRecipeRepository : IBaseRepository<Recipe>
{
    Task<Recipe?> GetRecipeByIdAsync(int id);
}
