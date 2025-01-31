using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Fitness_Tracker.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        
       

        // One-to-Many relationship with Diet
        public ICollection<Diet> Diets { get; set; } = new List<Diet>();

        // One-to-Many relationship with Progress
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();

        // One-to-Many relationship with Exercise
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();


    }
}
