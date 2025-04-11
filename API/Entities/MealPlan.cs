using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class MealPlan
    {
        [Key]
        public int MealPlanId { get; set; }
        
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        public ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = null!;
    }