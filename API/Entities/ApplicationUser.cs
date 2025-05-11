using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class ApplicationUser : IdentityUser<int>
{
        public string DisplayName { get; set; } = string.Empty;
        public Photo? Photo { get; set; } 
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
        public ICollection<MealPlan> MealPlans { get; set; } = null!;
        public ICollection<ShoppingList> ShoppingLists { get; set; } = null!;
        public DietaryPreferences DietaryPreferences { get; set; } = null!;
}
