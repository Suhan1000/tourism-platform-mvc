using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourismPlatform.Models;  

namespace TourismPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Tourist> Tourists { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Agency>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Agency>(a => a.UserId);

            modelBuilder.Entity<Tourist>()
                .HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<Tourist>(t => t.UserId);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Agency)
                .WithMany(a => a.Tours)
                .HasForeignKey(t => t.AgencyId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tour)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TourId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tourist)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TouristId);
        }
    }
}