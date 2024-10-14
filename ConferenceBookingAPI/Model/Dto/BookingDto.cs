using ConferenceBookingAPI.Model;

namespace ConferenceBookingAPI.Models.Dto
{
    public class BookingDto
    {
        public int? BookingId { get; set; }

        public string ApprovedBy { get; set; } = null!;

        public DateTime? BookingStart { get; set; }

        public DateTime? BookingEnd { get; set; }

        public string? Organizer { get; set; }  

        public string? Department { get; set; }

        public string? ContactNumber { get; set; }

        public string? EmailAddress { get; set; }

        public int? ExpectedAttendess { get; set; }

        public string Purpose { get; set; } = null!;

        public string? Description { get; set; }

        public string? Status { get; set; }

        public int ConferenceId { get; set; }

        public virtual Conference? Conference { get; set; } = null!;
    }
}
