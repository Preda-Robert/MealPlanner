using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

 public class Recipe
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public required string Difficulty { get; set; }
        public Photo? Photo { get; set; }
        public ICollection<RecipeInstruction> Instructions { get; set; } = [];
        public ICollection<RecipeIngredient> Ingredients { get; set; } = [];
        public int Servings { get; set; }
        public DateTime DateAdded { get; set; }
        public float Rating { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = [];
    }