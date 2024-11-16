namespace ConferenceBookingAPI.Model.Dto
{
    public class HolidayDto
    {
        public Guid? HolidayId { get; set; }
        public string? HolidayName { get; set; }
        public DateOnly? HolidayDate { get; set; }
    }
}
