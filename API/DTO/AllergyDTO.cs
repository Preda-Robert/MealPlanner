using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class AllergyDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
