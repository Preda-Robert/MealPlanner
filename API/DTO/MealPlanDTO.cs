using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class MealPlanDTO
{
  public int Id { get; set; }
  [Required]
  public required string Name { get; set; }
  [Required]
  public required string Description { get; set; }
  public string? PhotoUrl { get; set; }
  [Required]
  public DateTime StartDate { get; set; }
  [Required]
  public DateTime EndDate { get; set; }
  public int UserId { get; set; }

  public ICollection<MealPlanRecipeDTO> MealPlanRecipes { get; set; } = [];
}