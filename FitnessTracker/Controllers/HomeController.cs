using Fitness_Tracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Tracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch summary data for the dashboard
            var totalExercises = await _context.Exercises.CountAsync();
            var totalProgressEntries = await _context.Progresses.CountAsync();
            var totalCaloriesBurned = await _context.Exercises.SumAsync(e => e.CaloriesBurned);

            // Pass data to the view
            ViewBag.TotalExercises = totalExercises;
            ViewBag.TotalProgressEntries = totalProgressEntries;
            ViewBag.TotalCaloriesBurned = totalCaloriesBurned;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
