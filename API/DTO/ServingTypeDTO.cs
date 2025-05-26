using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class ServingTypeDTO
{
    public int Id { get; set; }
    public int NumberOfServings { get; set; }
    public bool Official { get; set; } = false;
    [MinLength(5)]
    [MaxLength(255)]
    [Required(ErrorMessage = "Description cannot be blank!")]
    public string Description { get; set; } = string.Empty;
}
