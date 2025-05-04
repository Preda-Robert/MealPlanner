using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class RecipeIngredient
    {
        [Key]
        public int RecipeIngredientId { get; set; }
        public int Quantity { get; set; }
        public required string Unit { get; set; }

        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; } = null!;
        public int IngredientId { get; set; }
        
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
