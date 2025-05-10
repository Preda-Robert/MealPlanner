using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class MealPlanRecipe
    {
        public int Id { get; set; }
        public Recipe Recipe { get; set; } = null!;
        public ServingType ServingType { get; set; } = null!;
    }