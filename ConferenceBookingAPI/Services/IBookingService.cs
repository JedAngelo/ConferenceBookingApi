using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IBookingService
    {
        Task<ApiResponse<string>> AddOrUpdateBooking(BookingDto dto);
        Task<ApiResponse<string>> DeleteBooking(Guid ID);
        Task<ApiResponse<List<BookingDto>>> GetAllBookings();
        Task<ApiResponse<BookingDto>> GetBookingByBookingId(Guid ID);
        Task<ApiResponse<List<BookingDto>>> GetBookingByConferenceId(Guid ID);
        Task<ApiResponse<List<BookingDto>>> GetBookingByDate(DateOnly date);
    }
}