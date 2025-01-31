using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models.Entities
{
    public class Diet
    {
        public int Id { get; set; }

        [Required]
        public string MealType { get; set; } // Breakfast, Lunch, etc.

        [Required]
        public int CaloriesConsumed { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow; // Default to UTC

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; } // Optional navigation property
    }

}
