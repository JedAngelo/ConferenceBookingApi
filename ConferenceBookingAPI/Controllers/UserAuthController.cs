﻿using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.UserAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthenticationService _authService;

        public UserAuthController(IUserAuthenticationService authService)
        {
            _authService = authService;
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpPost("RegisterSuperAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> RegisterSuperAdminAsync([FromBody] RegisterModelDto param)
        {
            var result = await _authService.RegisterSuperAdminAsync(param);
            return Ok(result);
        }

        //[Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpPost("RegisterAdmin")]
        public async Task<ActionResult<ApiResponse<string>>> RegisterAdmin([FromBody] RegisterModelDto param)
        {
            var result = await _authService.RegisterAdminAsync(param);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModelDto param)
        {
            var result = await _authService.RegisterUserAsync(param);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<UserLoginDto>>> Login([FromBody] LoginModelDto param)
        {
            var result = await _authService.LoginAsync(param);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetUserConferenceId")]
        public async Task<ActionResult<ApiResponse<int>>> GetUserConferenceId(string userId)
        {
            var result = await _authService.GetUserConferenceId(userId);
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpGet("GetAdmins")]
        public async Task<ActionResult<ApiResponse<List<UsersDto>>>> GetAdminsAsync()
        {
            var result = await _authService.GetAdminsAsync();
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpGet("GetUsers")]
        public async Task<ActionResult<ApiResponse<List<UsersDto>>>> GetUsersAsync(string? role = null)
        {
            var result = await _authService.GetUsersAsync(role);
            return Ok(result);
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpGet("RemoveUser/{userId}")]
        public async Task<ActionResult<ApiResponse<string>>> RemoveUserByIdAsync(string userId)
        {
            var result = await _authService.RemoveUserByIdAsync(userId);
            return Ok(result);
        }


    }
}
