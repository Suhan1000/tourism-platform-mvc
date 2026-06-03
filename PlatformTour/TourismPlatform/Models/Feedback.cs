using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismPlatform.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
