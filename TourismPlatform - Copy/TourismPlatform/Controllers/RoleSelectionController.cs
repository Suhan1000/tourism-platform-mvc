using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Data;
using TourismPlatform.Models;
using TourismPlatform.Services;

namespace TourismPlatform.Controllers
{
    /*public class RoleSelectionController : Controller
    {
        private readonly UserRoleService _userRoleService;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleSelectionController(UserRoleService userRoleService, UserManager<IdentityUser> userManager)
        {
            _userRoleService = userRoleService;
            _userManager = userManager;
        }
        private readonly ApplicationDbContext _context;

      
        // Show the role selection page
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Check if user already has a profile
                    var tourist = await _context.Tourists.FirstOrDefaultAsync(t => t.UserId == user.Id);
                    var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.UserId == user.Id);

                    if (tourist != null || agency != null)
                    {
                        // User already has profile, redirect to their dashboard
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }

            return View();
        }
        // Handle when user selects their role
        [HttpPost]
        public async Task<IActionResult> SelectRole(UserType userType)
        {
            // Check if user is logged in
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Create profile immediately
                    await _userRoleService.CreateUserProfileAsync(user.Id, userType, user.Email!);

                    // Show success message
                    TempData["SuccessMessage"] = $"Welcome! Your {userType} account has been created.";

                    // Redirect to dashboard
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            // If not logged in, store choice and redirect to registration
            HttpContext.Session.SetString("SelectedUserType", userType.ToString());
            TempData["InfoMessage"] = $"You selected: {userType}. Please complete registration below.";
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }
    }
} */


    // Trial Code
    public class RoleSelectionController : Controller
    {
        private readonly UserRoleService _userRoleService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleSelectionController(
            UserRoleService userRoleService,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userRoleService = userRoleService;
            _userManager = userManager;
            _context = context;
        }

        // Show the role selection page
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var tourist = await _context.Tourists
                        .FirstOrDefaultAsync(t => t.UserId == user.Id);
                    var agency = await _context.Agencies
                        .FirstOrDefaultAsync(a => a.UserId == user.Id);

                    if (tourist != null || agency != null)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }

            return View();
        }

        // Handle when user selects their role
        [HttpPost]
        public async Task<IActionResult> SelectRole(UserType userType)
        {
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    await _userRoleService.CreateUserProfileAsync(
                        user.Id, userType, user.Email!);

                    TempData["SuccessMessage"] = $"Welcome! Your {userType} account has been created.";

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            HttpContext.Session.SetString("SelectedUserType", userType.ToString());
            TempData["InfoMessage"] = $"You selected: {userType}. Please complete registration below.";
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }
    }
}

