using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    [Key]
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }

}
