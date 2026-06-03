using System.ComponentModel.DataAnnotations;

namespace TourismPlatform.Models
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public class Booking
    {
        public int Id { get; set; }
        public Feedback? Feedback { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        public int TouristId { get; set; }

        [Required]
        [Range(1, 50, ErrorMessage = "Number of people must be between 1 and 50")]
        [Display(Name = "Number of People")]
        public int NumberOfPeople { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Tour Date")]
        public DateTime TourDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Special Requests")]
        public string SpecialRequests { get; set; } = string.Empty;

        // Navigation Properties
        public Tour Tour { get; set; } = null!;
        public Tourist Tourist { get; set; } = null!;
        // Add this:
     
    }
}