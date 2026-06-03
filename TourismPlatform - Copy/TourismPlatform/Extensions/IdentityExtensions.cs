using Microsoft.AspNetCore.Identity;
using TourismPlatform.Models;
using TourismPlatform.Services;

namespace TourismPlatform.Extensions
{
    public static class IdentityExtensions
    {
        // This runs after successful registration
        public static async Task HandlePostRegistrationAsync(
            this UserManager<IdentityUser> userManager,
            IdentityUser user,
            HttpContext httpContext,
            UserRoleService userRoleService)
        {
            // Get the role they selected
            var selectedRole = httpContext.Session.GetString("SelectedUserType");

            if (!string.IsNullOrEmpty(selectedRole))
            {
                // Convert string back to enum
                if (Enum.TryParse<UserType>(selectedRole, out var userType))
                {
                    // Create appropriate profile
                    await userRoleService.CreateUserProfileAsync(user.Id, userType, user.Email!);

                    // Clear the session
                    httpContext.Session.Remove("SelectedUserType");
                }
            }
        }
    }
}