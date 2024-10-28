using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IConferenceService
    {
        Task<ApiResponse<string>> AddOrUpdateConference(ConferenceDto dto);
        Task<ApiResponse<string>> DeleteConference(long ID);
        Task<ApiResponse<List<ConferenceDto>>> GetAllConference();
        Task<ApiResponse<ConferenceDto>> GetConferenceById(int ID);

    }
}