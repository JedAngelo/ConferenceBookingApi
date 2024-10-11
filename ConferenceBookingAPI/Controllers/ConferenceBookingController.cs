using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceBookingController : ControllerBase
    {
        private readonly IConferenceBookingService _conferenceBookingService;

        public ConferenceBookingController(IConferenceBookingService conferenceBookingService)
        {
            _conferenceBookingService = conferenceBookingService;
        }



        #region Conference Controller
        [HttpPost("AddOrUpdateConference")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateConference(ConferenceDto dto)
        {
            var result = await _conferenceBookingService.AddOrUpdateConference(dto);
            return Ok(result);
        }

        [HttpDelete("DeleteConference/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteConference(long ID)
        {
            var result = await _conferenceBookingService.DeleteConference(ID);
            return Ok(result);
        }

        [HttpGet("GetAllConference")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllConference()
        {
            var result =await  _conferenceBookingService.GetAllConference();
            return Ok(result);
        }
        #endregion


        #region Booking Controller

        [HttpPost("AddOrUpdateBooking")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateBooking(BookingDto dto)
        {
            var result = await _conferenceBookingService.AddOrUpdateBooking(dto);
            return Ok(result);
        }

        [HttpDelete("DeleteBooking/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBooking(long ID)
        {
            var result = await _conferenceBookingService.DeleteBooking(ID);
            return Ok(result);
        }

        [HttpGet("GetAllBooking")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllBooking()
        {
            var result = await _conferenceBookingService.GetAllBookings();
            return Ok(result);
        }

        #endregion
    }
}
