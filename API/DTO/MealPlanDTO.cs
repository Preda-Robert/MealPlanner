using System;
using API.Entities;

namespace API.DTO;

public class MealPlanDTO
{
    public int Id { get; set; }
    public ICollection<MealPlanRecipeDTO> MealPlanRecipes { get; set; } = [];
}