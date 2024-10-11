using ConferenceBookingAPI.Models;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Model.Dto
{
    public class ConferenceDto
    {
        public int? ConferenceId { get; set; }

        public string? ConferenceName { get; set; }

        public int? Capacity { get; set; }

        public bool? IsActive { get; set; }

        public virtual ICollection<BookingDto> BookingDtos { get; set; } = new List<BookingDto>();
    }
}
