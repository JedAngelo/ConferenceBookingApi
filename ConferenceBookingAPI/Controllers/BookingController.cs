using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        #region Booking Controller

        [HttpPost("AddOrUpdateBooking")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateBooking(BookingDto dto)
        {
            var result = await _bookingService.AddOrUpdateBooking(dto);
            return Ok(result);
        }
        [HttpPost("UpdateBookingStatus")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateBookingStatuus(UpdateBookingStatusDto dto)
        {
            var result = await _bookingService.UpdateBookingStatus(dto);
            return Ok(result);
        }

        [HttpDelete("DeleteBooking/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBooking(long ID)
        {
            var result = await _bookingService.DeleteBooking(ID);
            return Ok(result);
        }

        [HttpGet("GetAllBooking")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllBooking()
        {
            var result = await _bookingService.GetAllBookings();
            return Ok(result);
        }

        [HttpGet("GetBookingByBookingID/{ID}")]
        public async Task<ActionResult<ApiResponse<BookingDto>>> GetBookingById(long ID)
        {
            var result = await _bookingService.GetBookingByBookingId(ID);
            return Ok(result);
        }

        [HttpGet("GetBookingByConferenceID/{ID}")]
        public async Task<ActionResult<List<ApiResponse<BookingDto>>>> GetBookingByConferenceId(long ID)
        {
            var result = await _bookingService.GetBookingByConferenceId(ID);
            return Ok(result);
        }

        #endregion

    }
}
