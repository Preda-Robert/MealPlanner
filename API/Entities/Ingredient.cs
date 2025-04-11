using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("Ingredients")]
public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public bool IsAllergen { get; set; }
        public required string NutritionalInfo { get; set; }
        public float Calories { get; set; }
        public Photo? Photo { get; set; } = null!;

        // Navigation properties
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = null!;
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; } = null!;
    }
