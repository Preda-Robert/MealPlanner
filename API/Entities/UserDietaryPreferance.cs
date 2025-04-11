using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class UserDietaryPreference
    {
        [Key]
        public int UserDietaryPreferenceId { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        public int DietaryPreferenceId { get; set; }
        
        [ForeignKey("DietaryPreferenceId")]
        public DietaryPreference DietaryPreference { get; set; } = null!;
    }