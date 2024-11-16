using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IConferenceService
    {
        Task<ApiResponse<string>> AddOrUpdateConference(ConferenceDto dto);
        Task<ApiResponse<string>> DeleteConference(Guid ID);
        Task<ApiResponse<List<ConferenceDto>>> GetAllConference();
        Task<ApiResponse<ConferenceDto>> GetConferenceById(Guid ID);
    }
}