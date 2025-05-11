using System;
using API.Controllers;
using API.DTO;
using API.Entities;

namespace API.Interfaces;

public interface IRecipeService : IBaseService<Recipe,RecipeDTO>
{

}
