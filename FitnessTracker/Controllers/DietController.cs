using Fitness_Tracker.Data;
using Fitness_Tracker.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Fitness_Tracker.Controllers
{
    public class DietController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context for accessing and managing the database
        private readonly UserManager<ApplicationUser> _userManager; // Manages user-related functionalities like getting logged-in user information

        // Constructor to inject dependencies
        public DietController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context; // Initialize the database context
            _userManager = userManager; // Initialize the user manager
        }

        // Action to display a list of diets for the logged-in user
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Get the currently logged-in user using UserManager
            var user = await _userManager.GetUserAsync(User);

            // If the user is not logged in, return an Unauthorized response
            if (user == null)
            {
                return Unauthorized();
            }

            // Query the database to get diets that belong to the logged-in user
            var diets = await _context.Diets
                .Where(d => d.UserId == user.Id) // Filter by the current user's ID
                .ToListAsync();

            // Pass the list of diets to the view for display
            return View(diets);
        }

        // Action to display the Add Diet form
        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Simply render the view for adding a new diet
        }

        // Action to handle form submission for adding a new diet
        [HttpPost]
        public async Task<IActionResult> Add(Diet diet)
        {
            // Get the currently logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            // If the user is not logged in, add a model error and redisplay the form
            if (user == null)
            {
                ModelState.AddModelError("", "You must be logged in to add a diet.");
                return View(diet);
            }

            // Assign the logged-in user's ID to the diet entry to associate it with the correct user
            diet.UserId = user.Id;

            // Add the diet to the database context and save changes
            _context.Diets.Add(diet);
            await _context.SaveChangesAsync();

            // Redirect the user to the list of diets after successfully adding the new diet
            return RedirectToAction("Get");
        }

        // Action to display the Edit Diet form for a specific diet entry
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get the currently logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            // If the user is not logged in, return an Unauthorized response
            if (user == null)
            {
                return Unauthorized();
            }

            // Find the diet entry by ID and ensure it belongs to the current user
            var diet = await _context.Diets
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == user.Id);

            // If the diet does not exist or does not belong to the user, return a NotFound response
            if (diet == null)
            {
                return NotFound();
            }

            // Pass the diet entry to the view for editing
            return View(diet);
        }

        // Action to handle form submission for editing an existing diet
        [HttpPost]
        public async Task<IActionResult> Edit(Diet diet)
        {
            // Get the currently logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            // If the user is not logged in or the diet does not belong to the user, return Unauthorized
            if (user == null || diet.UserId != user.Id)
            {
                return Unauthorized();
            }

            // Update the diet entry in the database and save changes
            _context.Diets.Update(diet);
            await _context.SaveChangesAsync();

            // Redirect the user to the list of diets after successfully editing
            return RedirectToAction("Get");
        }

        // Action to handle deletion of a diet entry
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the currently logged-in user's information
            var user = await _userManager.GetUserAsync(User);

            // If the user is not logged in, return an Unauthorized response
            if (user == null)
            {
                return Unauthorized();
            }

            // Find the diet entry by ID and ensure it belongs to the current user
            var diet = await _context.Diets
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == user.Id);

            // If the diet does not exist or does not belong to the user, return a NotFound response
            if (diet == null)
            {
                return NotFound();
            }

            // Remove the diet entry from the database context and save changes
            _context.Diets.Remove(diet);
            await _context.SaveChangesAsync();

            // Redirect the user to the list of diets after successfully deleting
            return RedirectToAction("Get");
        }
    }
}
