using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.DTO;

public class IngredientDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public IngredientCategoryDTO Category { get; set; } = null!;
    public bool IsAllergen { get; set; }
    [JsonPropertyName("MeasurementType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public IngredientMeasurementType MeasurementType { get; set; }
    public float Calories { get; set; }
    public PhotoDTO? Photo { get; set; } = null!;

}
