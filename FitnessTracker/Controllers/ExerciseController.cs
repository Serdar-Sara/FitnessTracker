using Fitness_Tracker.Data; 
using Fitness_Tracker.Models.Entities; 
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System; 
using System.Linq; // Provides LINQ functionality
using System.Threading.Tasks; // Provides asynchronous programming support

namespace Fitness_Tracker.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context for accessing the database
        private readonly UserManager<ApplicationUser> _userManager; // UserManager for handling user-related tasks

        // Constructor that initializes the database context and user manager
        public ExerciseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context; // Set the database context
            _userManager = userManager; // Set the user manager
        }

        // Action method to get the list of exercises for the logged-in user
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return 401 Unauthorized
            }

            // Fetch exercises belonging to the logged-in user
            var exercises = await _context.Exercises
                .Where(e => e.UserId == user.Id) // Filter by user's ID
                .ToListAsync();

            return View(exercises); // Pass the exercises to the view
        }

        // Action method to render the Add Exercise form
        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Render the Add Exercise view
        }

        // Action method to handle form submission for adding a new exercise
        [HttpPost]
        public async Task<IActionResult> Add(Exercise exercise)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) // Check if no user is logged in
            {
                ModelState.AddModelError("", "You must be logged in to add an exercise."); // Add an error message
                return View(exercise); // Re-render the Add form with the exercise data
            }

            // Assign the logged-in user's ID to the new exercise
            exercise.UserId = user.Id;

            try
            {
                // Add the exercise to the database and save changes
                _context.Exercises.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Get"); // Redirect to the Get action after successful addition
            }
            catch (Exception ex) // Handle any exceptions during the save process
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log the error message
                return View(exercise); // Re-render the Add form with the exercise data
            }
        }

        // Action method to render the Edit Exercise form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return 401 Unauthorized
            }

            // Find the exercise by ID and ensure it belongs to the logged-in user
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

            if (exercise == null) // Check if the exercise does not exist or does not belong to the user
            {
                return NotFound(); // Return 404 Not Found
            }

            return View(exercise); // Pass the exercise to the view for editing
        }

        // Action method to handle form submission for editing an exercise
        [HttpPost]
        public async Task<IActionResult> Edit(Exercise exercise)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null || exercise.UserId != user.Id) // Check if user is unauthorized to edit this exercise
            {
                return Unauthorized(); // Return 401 Unauthorized
            }

            try
            {
                // Update the exercise in the database and save changes
                _context.Exercises.Update(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Get"); // Redirect to the Get action after successful update
            }
            catch (Exception ex) // Handle any exceptions during the update process
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log the error message
                return View(exercise); // Re-render the Edit form with the exercise data
            }
        }

        // Action method to handle deletion of an exercise
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return 401 Unauthorized
            }

            // Find the exercise by ID and ensure it belongs to the logged-in user
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

            if (exercise == null) // Check if the exercise does not exist or does not belong to the user
            {
                return NotFound(); // Return 404 Not Found
            }

            try
            {
                // Remove the exercise from the database and save changes
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Get"); // Redirect to the Get action after successful deletion
            }
            catch (Exception ex) // Handle any exceptions during the delete process
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log the error message
                return RedirectToAction("Get"); // Redirect back to the Get action
            }
        }
    }
}
