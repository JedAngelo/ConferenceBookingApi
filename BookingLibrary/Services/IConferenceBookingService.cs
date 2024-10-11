using BookingLibrary.Models.DTO;

namespace BookingLibrary.Services
{
    public interface IConferenceBookingService
    {
        Task<ApiResponse<string>> InsertOrUpdateBooking(ConferenceBookingDto dto);
    }
}