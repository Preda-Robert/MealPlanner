using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class MealPlanRecipe
    {
        [Key]
        public int MealPlanRecipeId { get; set; }
        public required string MealType { get; set; }
        public DateTime PlannedDate { get; set; }
        public int MealPlanId { get; set; }

        [ForeignKey("MealPlanId")]
        public MealPlan MealPlan { get; set; } = null!;
        public int RecipeId { get; set; }
        
        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; } = null!;
    }