using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class DietaryPreferences
{
    public int Id { get; set; }
    public DietType DietType { get; set; } = null!;
    public ServingType ServingType { get; set; } = null!;
    public ICollection<Allergy> Allergies { get; set; } = [];
}