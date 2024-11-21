using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpPost("AddOrUpdateHoliday")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateHolidays(HolidayDto dto)
        {
            var result = await _holidayService.AddOrUpdateHolidays(dto);
            return Ok(result);
        }


        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpDelete("DeleteHoliday/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteHoliday(Guid ID)
        {
            var result = await _holidayService.DeleteHoliday(ID);
            return Ok(result);
        }


        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpGet("GetAllHoliday")]
        public async Task<ActionResult<ApiResponse<List<HolidayDto>>>> GetAllHoliday()
        {
            var result = await _holidayService.GetAllHoliday();
            return Ok(result);
        }


        [Authorize(Roles = UserRoles.AdminRole + "," + UserRoles.SuperAdmin)]
        [HttpGet("GetHolidayById/{ID}")]
        public async Task<ActionResult<ApiResponse<HolidayDto>>> GetHolidayById(Guid ID)
        {
            var result = await _holidayService.GetHolidayById(ID);
            return Ok(result);
        }
    }
}
