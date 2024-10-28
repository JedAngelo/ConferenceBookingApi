using Microsoft.AspNetCore.Identity;

namespace ConferenceBookingAPI.Model.Dto.UserAuthDto
{
    public class AdminUsersDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
