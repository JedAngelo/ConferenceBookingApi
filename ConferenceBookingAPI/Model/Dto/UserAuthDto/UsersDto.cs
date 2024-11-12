using Microsoft.AspNetCore.Identity;

namespace ConferenceBookingAPI.Model.Dto.UserAuthDto
{
    public class UsersDto
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public int? ConferenceId { get; set; }
        public IList<string>? UserRole { get; set; }
    }
}
