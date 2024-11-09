using ConferenceBookingAPI.Model;
using Microsoft.AspNetCore.Identity;

namespace ConferenceBookingAPI.UserAuth
{
    public class ApplicationUser : IdentityUser
    {
        // This represents the foreign key relationship to Conference
        public int? ConferenceId { get; set; } // Foreign key for the associated Conference
        public virtual Conference? Conference { get; set; } // Navigation property
    }
}
