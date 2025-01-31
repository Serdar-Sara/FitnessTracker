using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models.Entities
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Duration { get; set; }

        public int CaloriesBurned { get; set; }

        [Required]
        public DateTime Date { get; set; }


        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; } // Navigation property
    }

}
