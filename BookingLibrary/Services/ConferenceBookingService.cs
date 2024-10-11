using BookingLibrary.Models;
using BookingLibrary.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingLibrary.Services
{
    public class ConferenceBookingService : IConferenceBookingService
    {
        private readonly ConferenceDbContext _context;

        public ConferenceBookingService(ConferenceDbContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<string>> InsertOrUpdateBooking(ConferenceBookingDto dto)
        {
            try
            {
                if (dto.ConferenceID == null)
                {
                    var _booking = new ConferenceBooking()
                    {
                        ConferenceID = Guid.NewGuid(),
                        Schedule = dto.Schedule,
                        Meeting = dto.Meeting
                    };
                    await _context.ConferenceBookings.AddAsync(_booking);
                    await _context.SaveChangesAsync();

                    var _result = new ApiResponse<string>()
                    {
                        Data = "Success",
                        ErrorMessage = "",
                        IsSucces = true
                    };

                    return _result;
                }
                else
                {
                    var _booking = new ConferenceBooking()
                    {
                        Schedule = dto.Schedule,
                        Meeting = dto.Meeting
                    };
                    _context.ConferenceBookings.Update(_booking);
                    await _context.SaveChangesAsync();

                    var _result = new ApiResponse<string>()
                    {
                        Data = "Success",
                        ErrorMessage = "",
                        IsSucces = true
                    };

                    return _result;
                }

            }
            catch (Exception ex)
            {

                var _result = new ApiResponse<string>()
                {
                    Data = "Failed",
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSucces = false
                };

                return _result;
            }
        }
    }
}
