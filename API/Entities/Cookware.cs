using System.ComponentModel.DataAnnotations;

namespace API.Entities;

 public class Cookware
    {
        [Key]
        public int CookwareId { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string Description { get; set; }
        public Photo? Photo { get; set; } = null!;

        // Navigation properties
        public ICollection<RecipeCookware> RecipeCookwares { get; set; } = null!;
    }