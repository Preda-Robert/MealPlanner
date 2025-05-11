using System;
using API.Entities;

namespace API.DTO;

public class RecipeIngredientDTO
{
        public int Quantity { get; set; }
        public IngredientDTO Ingredient { get; set; } = null!;
}
