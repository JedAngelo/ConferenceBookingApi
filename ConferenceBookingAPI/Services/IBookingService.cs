using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IBookingService
    {
        Task<ApiResponse<string>> AddOrUpdateBooking(BookingDto dto);
        Task<ApiResponse<string>> DeleteBooking(long ID);
        Task<ApiResponse<List<BookingDto>>> GetAllBookings();
        Task<ApiResponse<BookingDto>> GetBookingByBookingId(long ID);
        Task<ApiResponse<List<BookingDto>>> GetBookingByConferenceId(long ID);
        Task<ApiResponse<string>> UpdateBookingStatus(UpdateBookingStatusDto dto);

    }
}