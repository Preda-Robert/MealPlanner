using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class ApplicationUser : IdentityUser<int>
{
        public Photo? Photo { get; set; } 
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
        public ICollection<MealPlan> MealPlans { get; set; } = null!;
        public ICollection<ShoppingList> ShoppingLists { get; set; } = null!;
        public ICollection<UserDietaryPreference> UserDietaryPreferences { get; set; } = null!;
}
