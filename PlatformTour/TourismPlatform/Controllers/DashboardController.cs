/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Data;
using TourismPlatform.Models;

namespace TourismPlatform.Controllers
{
    [Authorize] // Only logged-in users can access
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Main dashboard - redirects to appropriate dashboard based on user type
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            // Check if user is a tourist
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (tourist != null)
            {
                return RedirectToAction("Tourist");
            }

            // Check if user is agency or tour guide
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (agency != null)
            {
                if (agency.UserType == UserType.TravelAgency)
                {
                    return RedirectToAction("Agency");
                }
                else // TourGuide
                {
                    return RedirectToAction("TourGuide");
                }
            }

            // If no profile found, redirect to role selection
            return RedirectToAction("Index", "RoleSelection");
        }

        // Travel Agency Dashboard
        public async Task<IActionResult> Agency()
        {
            var user = await _userManager.GetUserAsync(User);
            var agency = await _context.Agencies
                .Include(a => a.Tours)
                .FirstOrDefaultAsync(a => a.UserId == user!.Id);

            if (agency == null) return RedirectToAction("Index", "RoleSelection");

            ViewBag.UserType = "Agency";
            return View("AgencyDashboard", agency);
        }

        // Tour Guide Dashboard  
        public async Task<IActionResult> TourGuide()
        {
            var user = await _userManager.GetUserAsync(User);
            var guide = await _context.Agencies
                .Include(a => a.Tours)
                .FirstOrDefaultAsync(a => a.UserId == user!.Id);

            if (guide == null) return RedirectToAction("Index", "RoleSelection");

            ViewBag.UserType = "TourGuide";
            return View("TourGuideDashboard", guide);
        }

        // Tourist Dashboard
        public async Task<IActionResult> Tourist()
        {
            var user = await _userManager.GetUserAsync(User);
            var tourist = await _context.Tourists
                .Include(t => t.Bookings)
                .ThenInclude(b => b.Tour)
                .FirstOrDefaultAsync(t => t.UserId == user!.Id);

            if (tourist == null) return RedirectToAction("Index", "RoleSelection");

            ViewBag.UserType = "Tourist";
            return View("TouristDashboard", tourist);
        }
    }
} */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Data;
using TourismPlatform.Models;

namespace TourismPlatform.Controllers
{
    [Authorize] // Only logged-in users can access
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Main dashboard - redirects to appropriate dashboard based on user type
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            // Check if user is a tourist
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (tourist != null)
            {
                return RedirectToAction("Tourist");
            }

            // Check if user is agency or tour guide
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (agency != null)
            {
                if (agency.UserType == UserType.TravelAgency)
                {
                    return RedirectToAction("Agency");
                }
                else
                {
                    return RedirectToAction("TourGuide");
                }
            }

            // Only show role selection if no profile exists
            return RedirectToAction("Index", "RoleSelection");
        }
        // Travel Agency Dashboard - ONLY for agencies
        public async Task<IActionResult> Agency()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var agency = await _context.Agencies
                .Include(a => a.Tours)
                .ThenInclude(t => t.Bookings)
                .FirstOrDefaultAsync(a => a.UserId == user.Id && a.UserType == UserType.TravelAgency);

            if (agency == null)
            {
                TempData["ErrorMessage"] = "Access denied. This dashboard is only for Travel Agencies.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserType = "Agency";
            return View("AgencyDashboard", agency);
        }

        // Tour Guide Dashboard - ONLY for tour guides
        public async Task<IActionResult> TourGuide()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            // Use Agencies table with UserType == TourGuide
            var guide = await _context.Agencies
                .Include(g => g.Tours)
                .ThenInclude(t => t.Bookings)
                .FirstOrDefaultAsync(g => g.UserId == user.Id && g.UserType == UserType.TourGuide);

            if (guide == null)
            {
                TempData["ErrorMessage"] = "Access denied. This dashboard is only for Tour Guides.";
                return RedirectToAction("Index", "Home");
            }

            // Flatten all bookings from all tours assigned to this guide
            var assignedBookings = guide.Tours
                .SelectMany(t => t.Bookings)
                .ToList();

            // Pass the guide and assigned bookings via ViewBag
            ViewBag.GuideName = guide.AgencyName;
            ViewBag.AssignedBookings = assignedBookings;

            return View("TourGuideDashboard", guide);
        }

        // Tourist Dashboard - ONLY for tourists
        public async Task<IActionResult> Tourist()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var tourist = await _context.Tourists
                .Include(t => t.Bookings)
                .ThenInclude(b => b.Tour)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (tourist == null)
            {
                TempData["ErrorMessage"] = "Access denied. This dashboard is only for Tourists.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserType = "Tourist";
            return View("TouristDashboard", tourist);
        }

        // Helper method to check user access (can be used elsewhere)
        private async Task<UserType?> GetUserTypeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            // Check tourist
            var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (tourist != null) return UserType.Tourist;

            // Check agency/guide
            var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (agency != null) return agency.UserType;

            return null;
        }
    }
}