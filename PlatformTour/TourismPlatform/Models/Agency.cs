using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TourismPlatform.Models
{
    public class Agency
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your agency name")]
        [StringLength(100)]
        [Display(Name = "Agency Name")]
        public string AgencyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a description")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter contact information")]
        [StringLength(200)]
        [Display(Name = "Contact Information")]
        public string ContactInfo { get; set; } = string.Empty;

        [StringLength(100)]
        public string Website { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // User type identification  
        public UserType UserType { get; set; } = UserType.TravelAgency;

        // Limitation for tour guides
        public int MaxToursAllowed { get; set; } = 999;

        // Navigation Properties
        public IdentityUser User { get; set; } = null!;
        public List<Tour> Tours { get; set; } = new List<Tour>();
    }
}