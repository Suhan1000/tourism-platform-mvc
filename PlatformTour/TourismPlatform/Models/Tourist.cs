using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TourismPlatform.Models
{
    public class Tourist
    {
        public int Id { get; set; }

        // Link to the user account
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties  
        public IdentityUser User { get; set; } = null!;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}