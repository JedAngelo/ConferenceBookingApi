using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;

namespace ConferenceBookingAPI.UserAuth
{
    public interface IUserAuthenticationService
    {
        Task<ApiResponse<UserLoginDto>> LoginAsync(LoginModelDto param);
        Task<ApiResponse<string>> RegisterSuperAdminAsync(RegisterModelDto param);
        Task<ApiResponse<string>> RegisterAdminAsync(RegisterModelDto param);
        Task<ApiResponse<string>> RegisterUserAsync(RegisterModelDto param);
        Task<ApiResponse<List<UsersDto>>> GetAdminsAsync();
        Task<ApiResponse<List<UsersDto>>> GetUsersAsync(string role = null);
        Task<ApiResponse<int>> GetUserConferenceId(string userId);
        Task<ApiResponse<string>> RemoveUserByIdAsync(string userId);
    }
}