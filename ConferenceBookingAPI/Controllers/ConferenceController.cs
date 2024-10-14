using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.Services;
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
            return Ok(result);
        }

        [HttpDelete("DeleteConference/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteConference(long ID)
        {
            var result = await _conferenceService.DeleteConference(ID);
            return Ok(result);
        }

        [HttpGet("GetAllConference")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllConference()
        {
            var result = await _conferenceService.GetAllConference();
            return Ok(result);
        }
        #endregion
    }
}
