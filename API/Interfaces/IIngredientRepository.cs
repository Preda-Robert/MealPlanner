using System;
using API.Entities;

namespace API.Interfaces;

public interface IIngredientRepository : IBaseRepository<Ingredient>
{
    Task<Ingredient?> GetIngredientByNameAsync(string name);
}
