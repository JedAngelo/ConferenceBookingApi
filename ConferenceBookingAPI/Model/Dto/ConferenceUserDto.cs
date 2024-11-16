using ConferenceBookingAPI.UserAuth;

namespace ConferenceBookingAPI.Model.Dto
{
    public class ConferenceUserDto
    {
        public int ConferenceUserId { get; set; }
        public Guid? ConferenceId { get; set; }
        public Conference? Conference { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
