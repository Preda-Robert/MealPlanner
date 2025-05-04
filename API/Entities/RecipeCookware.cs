using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class RecipeCookware
    {
        [Key]
        public int RecipeCookwareId { get; set; }
        public int RecipeId { get; set; }
        public int CookwareId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; } = null!;
        
        [ForeignKey("CookwareId")]
        public Cookware Cookware { get; set; } = null!;
    }
