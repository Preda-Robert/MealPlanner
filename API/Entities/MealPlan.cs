namespace API.Entities;

public class MealPlan
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Photo? Photo { get; set; } = null!;
  public ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = [];
  public int UserId { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }

  public ApplicationUser User { get; set; } = null!;
}