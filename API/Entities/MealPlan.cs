using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components;

namespace API.Entities;

public class MealPlan
{
    public int Id { get; set; }        
    public ICollection<MealPlanRecipe> MealPlanRecipes { get; set; } = [];
}