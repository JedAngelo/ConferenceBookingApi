using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        private readonly IConferenceService _conferenceService;

        public ConferenceController(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
        }

        #region Conference Controller
        [HttpPost("AddOrUpdateConference")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateConference(ConferenceDto dto)
        {
            var result = await _conferenceService.AddOrUpdateConference(dto);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage == "A conference with this name already exists.")
                {
                    return Conflict(result);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpDelete("DeleteConference/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteConference(long ID)
        {
            var result = await _conferenceService.DeleteConference(ID);
            return Ok(result);
        }
        //[Authorize(Roles = UserRoles.AdminRole)]
        [HttpGet("GetAllConference")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllConference()
        {
            var result = await _conferenceService.GetAllConference();
            return Ok(result);
        }

        [HttpGet("GetConferenceById/{ID}")]
        public async Task<ActionResult<ApiResponse<ConferenceDto>>> GetConferenceById(int ID)
        {
            var result = await _conferenceService.GetConferenceById(ID);
            return Ok(result);
        }
        #endregion
    }
}
