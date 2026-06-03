using Microsoft.AspNetCore.Identity;
using TourismPlatform.Data;
using TourismPlatform.Models;

namespace TourismPlatform.Services
{
    public class UserRoleService
    {
        private readonly ApplicationDbContext _context;

        public UserRoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create user profile based on their selected type
        public async Task CreateUserProfileAsync(string userId, UserType userType, string email)
        {
            if (userType == UserType.Tourist)
            {
                // Create tourist profile
                var tourist = new Tourist
                {
                    UserId = userId,
                    FirstName = "New",  // Will be updated by user later
                    LastName = "Tourist",
                    CreatedDate = DateTime.Now
                };

                _context.Tourists.Add(tourist);
            }
            else // TravelAgency or TourGuide
            {
                // Create agency/guide profile  
                var agency = new Agency
                {
                    UserId = userId,
                    AgencyName = email.Split('@')[0], // Use email prefix as temporary name
                    Description = "Please update your profile",
                    ContactInfo = email,
                    UserType = userType,
                    MaxToursAllowed = userType == UserType.TravelAgency ? 999 : 10,
                    CreatedDate = DateTime.Now
                };

                _context.Agencies.Add(agency);
            }

            await _context.SaveChangesAsync();
        }
    }
}
