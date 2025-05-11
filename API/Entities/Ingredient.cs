using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("Ingredients")]
public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IngredientCategory Category { get; set; } = null!;
    public bool IsAllergen { get; set; }
    public IngredientMeasurementType MeasurementType { get; set; }
    public float Calories { get; set; }
    public Photo? Photo { get; set; } = null!;
}
