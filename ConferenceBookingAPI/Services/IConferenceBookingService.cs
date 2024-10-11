using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IConferenceBookingService
    {
        Task<ApiResponse<string>> AddOrUpdateBooking(BookingDto dto);
        Task<ApiResponse<string>> AddOrUpdateConference(ConferenceDto dto);
        Task<ApiResponse<string>> DeleteBooking(long ID);
        Task<ApiResponse<string>> DeleteConference(long ID);
        Task<ApiResponse<List<BookingDto>>> GetAllBookings();
        Task<ApiResponse<List<ConferenceDto>>> GetAllConference();
    }
}