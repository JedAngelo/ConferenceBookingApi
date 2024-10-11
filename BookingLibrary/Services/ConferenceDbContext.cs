using BookingLibrary.Models;
using Microsoft.EntityFrameworkCore;


namespace BookingLibrary.Services
{
    public class ConferenceDbContext : DbContext
    {
        public ConferenceDbContext(DbContextOptions<ConferenceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ConferenceBooking> ConferenceBookings { get; set; }
    }
}
