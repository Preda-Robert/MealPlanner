using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class DietaryPreference
    {
        [Key]
        public int DietaryPreferenceId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // Navigation properties
        public ICollection<UserDietaryPreference> UserDietaryPreferences { get; set; } = null!;
    }