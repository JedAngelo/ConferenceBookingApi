using BookingLibrary.Models.DTO;
using BookingLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceBookingController : ControllerBase
    {
        private readonly IConferenceBookingService _conferenceBooking;

        public ConferenceBookingController(IConferenceBookingService conferenceBooking)
        {
            _conferenceBooking = conferenceBooking;
        }

        [HttpPost("InsertOrUpdateBooking")]
        public async Task<ActionResult<ApiResponse<string>>> InsertOrUpdateBooking(ConferenceBookingDto dto)
        {
            var result = await _conferenceBooking.InsertOrUpdateBooking(dto);
            return Ok(result);
        }
    }
}
