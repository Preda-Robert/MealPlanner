using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class ServingType
{
    public int Id { get; set; }
    public int NumberOfServings { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Official { get; set; } = false;
    public ICollection<Recipe> Recipes { get; set; } = [];
}
