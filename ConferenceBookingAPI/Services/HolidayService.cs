using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.UserAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ConferenceBookingAPI.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly ApplicationDbContext _context;

        public HolidayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> AddOrUpdateHolidays(HolidayDto dto)
        {
            try
            {
                if (dto.HolidayId == null)
                {
                    var _holiday = new Holiday
                    {
                        HolidayId = new Guid(),
                        HolidayName = dto.HolidayName!,
                        HolidayDate = (DateOnly)dto.HolidayDate!
                    };

                    await _context.Holidays.AddAsync(_holiday);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>
                    {
                        Data = "Successfully added holiday",
                        ErrorMessage = "",
                        IsSuccess = true
                    };

                }
                else
                {
                    var _existingHoliday = await _context.Holidays.FirstOrDefaultAsync(h => h.HolidayId == dto.HolidayId);

                    if (_existingHoliday == null)
                    {
                        return new ApiResponse<string>
                        {
                            Data = "",
                            ErrorMessage = "Holiday does not exist",
                            IsSuccess = false
                        };
                    }

                    _existingHoliday.HolidayName = dto.HolidayName ?? _existingHoliday.HolidayName;
                    _existingHoliday.HolidayDate = dto.HolidayDate ?? _existingHoliday.HolidayDate;

                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>
                    {
                        Data = "Successfully updated holiday",
                        ErrorMessage = "",
                        IsSuccess = true
                    };
                }

            }
            catch (Exception ex)
            {

                return new ApiResponse<string>
                {
                    Data = "",
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteHoliday(Guid ID)
        {
            try
            {
                var _holiday = await _context.Holidays.FirstOrDefaultAsync(h => h.HolidayId == ID);
                if (_holiday == null)
                {
                    return new ApiResponse<string>
                    {
                        Data = "",
                        ErrorMessage = "Holiday does not exist",
                        IsSuccess = false
                    };
                }
                else
                {
                    _context.Holidays.Remove(_holiday);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<string>
                    {
                        Data = "Successfully delete holiday",
                        ErrorMessage = "",
                        IsSuccess = true
                    };
                }

            }
            catch (Exception ex)
            {

                return new ApiResponse<string>
                {
                    Data = "",
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ApiResponse<List<HolidayDto>>> GetAllHoliday()
        {
            try
            {
                var _holidays = await _context.Holidays.Select(h => new HolidayDto
                {
                    HolidayId = h.HolidayId,
                    HolidayName = h.HolidayName,
                    HolidayDate = h.HolidayDate
                }).ToListAsync();

                return new ApiResponse<List<HolidayDto>>
                {
                    Data = _holidays,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<HolidayDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ApiResponse<HolidayDto>> GetHolidayById(Guid ID)
        {
            try
            {
                var _holiday = await _context.Holidays.Where(x => x.HolidayId == ID).Select(h => new HolidayDto
                {
                    HolidayId = h.HolidayId,
                    HolidayName = h.HolidayName,
                    HolidayDate = h.HolidayDate
                }).FirstOrDefaultAsync();

                return new ApiResponse<HolidayDto>
                {
                    Data = _holiday,
                    ErrorMessage = "",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<HolidayDto>
                {
                    Data = null,
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
    }
}
