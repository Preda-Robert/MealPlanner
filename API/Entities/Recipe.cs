using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

 public class Recipe
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int CookingTime { get; set; }
        public Photo? Photo { get; set; }
        public float Rating { get; set; }
        public RecipeDifficulty Difficulty { get; set; }
        public ServingType ServingType { get; set; } = null!;
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public ICollection<RecipeCookware> Cookware { get; set; } = [];
        public ICollection<RecipeInstruction> Instructions { get; set; } = [];
        public ICollection<RecipeIngredient> Ingredients { get; set; } = [];
    }