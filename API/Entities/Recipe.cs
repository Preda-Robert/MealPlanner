using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

 public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public required string Difficulty { get; set; }
        public required string Image { get; set; }
        public required string Instructions { get; set; }
        public int Servings { get; set; }
        public DateTime DateAdded { get; set; }
        public float Rating { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = null!;
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = null!;
        public ICollection<RecipeCookware> RecipeCookwares { get; set; } = null!;
    }