using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models.Entities
{
    public class Progress
    {
       
        public int Id { get; set; }

        [Required]
        public double Weight { get; set; }  // Weight in kg

        [Required]
        public double BodyFatPercentage { get; set; }  // Body fat percentage

        [Required]
        public DateTime Date { get; set; }  // Date of record

        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }  // Navigation property to the User
    }
}
