using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.Services
{
    public interface IHolidayService
    {
        Task<ApiResponse<string>> AddOrUpdateHolidays(HolidayDto dto);
        Task<ApiResponse<string>> DeleteHoliday(Guid ID);
        Task<ApiResponse<List<HolidayDto>>> GetAllHoliday();
        Task<ApiResponse<HolidayDto>> GetHolidayById(Guid ID);
    }
}