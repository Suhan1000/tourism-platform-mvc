using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourismPlatform.Models
{
    public class Tour
    {
        // Unique ID for each tour (like a barcode)
        public int Id { get; set; }

        // New property to store image file path
        public string? ImagePath { get; set; }

        // Tour name - like "Amazing Paris Adventure"
        [Required(ErrorMessage = "Please enter a tour name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Tour Name")]
        public string Name { get; set; } = string.Empty;

        // What tourists will do on this tour
        [Required(ErrorMessage = "Please provide a description")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        // How much the tour costs
        [Required(ErrorMessage = "Please set a price")]
        [Range(1, 50000, ErrorMessage = "Price must be between $1 and $50,000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Price ($)")]
        public decimal Price { get; set; }

        // How many days the tour lasts
        [Required(ErrorMessage = "Please specify duration")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        [Display(Name = "Duration (days)")]
        public int Duration { get; set; }

        // Maximum number of people allowed
        [Required(ErrorMessage = "Please set maximum group size")]
        [Range(1, 100)]
        [Display(Name = "Maximum Participants")]
        public int MaxParticipants { get; set; }
        //public int MaxParticipants { get; set; }

        // Which city/destination
        [Required(ErrorMessage = "Please specify the destination")]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        // When this tour was created (automatic)
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Is this tour still available for booking?
        [Display(Name = "Available for Booking")]
        public bool IsActive { get; set; } = true;
        // Link tours to agencies
        [Required]
        public int AgencyId { get; set; }

        // Navigation Properties
        [ValidateNever]
        public Agency Agency { get; set; } = null!;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    
    }
}