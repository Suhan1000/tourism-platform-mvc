using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Data;
using TourismPlatform.Models;

namespace TourismPlatform.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Show booking form
        public async Task<IActionResult> Create(int tourId)
        {
            var tour = await _context.Tours.Include(t => t.Agency).FirstOrDefaultAsync(t => t.Id == tourId);
            if (tour == null) return NotFound();

            ViewBag.Tour = tour;
            return View();
        }

        // Process booking
        [HttpPost]
        public async Task<IActionResult> Create(int tourId, int numberOfPeople, DateTime tourDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (tourist == null)
            {
                TempData["ErrorMessage"] = "Only tourists can make bookings.";
                return RedirectToAction("Index", "Tours");
            }

            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
            if (tour == null) return NotFound();

            var booking = new Booking
            {
                TourId = tourId,
                TouristId = tourist.Id,
                NumberOfPeople = numberOfPeople,
                TourDate = tourDate,
                TotalAmount = tour.Price * numberOfPeople,
                Status = BookingStatus.Pending
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking created successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // View user's bookings
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (tourist == null) return RedirectToAction("Index", "Tours");

            var bookings = await _context.Bookings
                .Include(b => b.Tour)
                .ThenInclude(t => t.Agency)
                .Where(b => b.TouristId == tourist.Id)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }
    }
}