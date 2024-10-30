using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.UserAuth;

namespace ConferenceBookingAPI.Model.Dto
{
    public class ConferenceDto
    {
        public int? ConferenceId { get; set; }

        public string? ConferenceName { get; set; }

        public int? Capacity { get; set; }

        public bool? IsActive { get; set; }

        public virtual List<BookingDto> BookingDtos { get; set; } = new List<BookingDto>();

        public virtual List<AdminUsersDto>? AdminUsers { get; set; } = new List<AdminUsersDto>();
    }
}
