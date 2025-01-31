using Fitness_Tracker.Data; 
using Fitness_Tracker.Models.Entities; 
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks; 

namespace Fitness_Tracker.Controllers
{
    public class ProgressController : Controller
    {
        private readonly ApplicationDbContext _context; // Database context for accessing and manipulating data
        private readonly UserManager<ApplicationUser> _userManager; // UserManager for managing user authentication and operations

        // Constructor to initialize the database context and UserManager
        public ProgressController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context; // Assign the passed-in database context
            _userManager = userManager; // Assign the passed-in user manager
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return HTTP 401 Unauthorized if the user is not logged in
            }

            // Fetch the list of progress records belonging to the logged-in user
            var progresses = await _context.Progresses
                .Where(p => p.UserId == user.Id) // Filter by UserId to ensure only the user's progress is retrieved
                .ToListAsync();

            return View(progresses); // Pass the list of progresses to the view for rendering
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Render the Add Progress view
        }

        [HttpPost]
        public async Task<IActionResult> Add(Progress progress)
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return HTTP 401 Unauthorized if the user is not logged in
            }

            // Set the UserId of the progress to the logged-in user's ID
            progress.UserId = user.Id;

            try
            {
                // Add the progress record to the database
                await _context.Progresses.AddAsync(progress);
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction("Get"); // Redirect to the Get action to show the list of progresses
            }
            catch
            {
                // Handle any errors during the save operation
                ModelState.AddModelError("", "An error occurred while adding the progress.");
                return RedirectToAction("Get"); // Redirect to the Get action in case of an error
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return HTTP 401 Unauthorized if the user is not logged in
            }

            // Retrieve the progress record by ID and ensure it belongs to the logged-in user
            var progress = await _context.Progresses
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);

            if (progress == null) // Check if the progress record does not exist or does not belong to the user
            {
                return NotFound(); // Return HTTP 404 Not Found if no matching progress record is found
            }

            return View(progress); // Pass the progress record to the view for editing
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Progress progress)
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null || progress.UserId != user.Id) // Check if the user is unauthorized to edit this progress record
            {
                return Unauthorized(); // Return HTTP 401 Unauthorized
            }

            try
            {
                // Update the progress record in the database
                _context.Progresses.Update(progress);
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction("Get"); // Redirect to the Get action to show the updated list of progresses
            }
            catch
            {
                // Handle any errors during the update operation
                ModelState.AddModelError("", "An error occurred while updating the progress.");
                return RedirectToAction("Get"); // Redirect to the Get action in case of an error
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null) // Check if no user is logged in
            {
                return Unauthorized(); // Return HTTP 401 Unauthorized if the user is not logged in
            }

            // Retrieve the progress record by ID and ensure it belongs to the logged-in user
            var progress = await _context.Progresses
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == user.Id);

            if (progress == null) // Check if the progress record does not exist or does not belong to the user
            {
                return NotFound(); // Return HTTP 404 Not Found if no matching progress record is found
            }

            try
            {
                // Remove the progress record from the database
                _context.Progresses.Remove(progress);
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction("Get"); // Redirect to the Get action to show the updated list of progresses
            }
            catch
            {
                // Handle any errors during the delete operation
                ModelState.AddModelError("", "An error occurred while deleting the progress.");
                return RedirectToAction("Get"); // Redirect to the Get action in case of an error
            }
        }
    }
}
