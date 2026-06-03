using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourismPlatform.Extensions;
using TourismPlatform.Services;

namespace TourismPlatform.Controllers
{
    public class RegistrationHandler : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserRoleService _userRoleService;

        public RegistrationHandler(UserManager<IdentityUser> userManager, UserRoleService userRoleService)
        {
            _userManager = userManager;
            _userRoleService = userRoleService;
        }

        // This gets called after successful registration
        public async Task<IActionResult> CompleteRegistration(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Create user profile based on selected role
                await _userManager.HandlePostRegistrationAsync(user, HttpContext, _userRoleService);

                // Show success message and redirect to appropriate dashboard
                TempData["WelcomeMessage"] = "Welcome! Your account has been created successfully.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}