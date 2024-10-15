using Microsoft.AspNetCore.Identity;

namespace ConferenceBookingAPI.UserAuth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
