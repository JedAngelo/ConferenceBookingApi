using ConferenceBookingAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingAPI.UserAuth
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Conference and ApplicationUser
            modelBuilder.Entity<Conference>()
                .HasMany(c => c.ApplicationUser)
                .WithOne(u => u.Conference)
                .HasForeignKey(u => u.ConferenceId)
                .OnDelete(DeleteBehavior.SetNull); // Or use another behavior as needed
        }
    }
}
