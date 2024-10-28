namespace ConferenceBookingAPI.Model.Dto
{
    public class UpdateBookingStatusDto
    {
        public int? BookingId { get; set; }
        public string? Status { get; set; }

        public string? ApprovedBy { get; set; }

    }
}
