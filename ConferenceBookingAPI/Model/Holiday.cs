namespace ConferenceBookingAPI.Model
{
    public class Holiday
    {
        public Guid HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateOnly HolidayDate { get; set; }
    }
}
