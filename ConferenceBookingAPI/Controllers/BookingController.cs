﻿using ConferenceBookingAPI.Model.Dto;
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
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        #region Booking Controller
        [AllowAnonymous]
        [HttpPost("AddOrUpdateBooking")]
        public async Task<ActionResult<ApiResponse<string>>> AddOrUpdateBooking(BookingDto dto)
        {
            var result = await _bookingService.AddOrUpdateBooking(dto);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("DeleteBooking/{ID}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBooking(Guid ID)
        {
            var result = await _bookingService.DeleteBooking(ID);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllBooking")]
        public async Task<ActionResult<ApiResponse<List<ConferenceDto>>>> GetAllBooking()
        {
            var result = await _bookingService.GetAllBookings();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetBookingByBookingID/{ID}")]
        public async Task<ActionResult<ApiResponse<BookingDto>>> GetBookingById(Guid ID)
        {
            var result = await _bookingService.GetBookingByBookingId(ID);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetBookingByConferenceID/{ID}")]
        public async Task<ActionResult<List<ApiResponse<BookingDto>>>> GetBookingByConferenceId(Guid ID)
        {
            var result = await _bookingService.GetBookingByConferenceId(ID);
            return Ok(result);
        }

        #endregion

        

    }
}
