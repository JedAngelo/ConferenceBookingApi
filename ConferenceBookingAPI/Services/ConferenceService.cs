using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingAPI.Services
{
    public class ConferenceService : IConferenceService
    {
        private readonly ConferenceBookingContext _context;

        public ConferenceService(ConferenceBookingContext context)
        {
            _context = context;
        }


        #region Conference Services

        public async Task<ApiResponse<string>> AddOrUpdateConference(ConferenceDto dto)
        {
            try
            {

                if (dto.ConferenceId == null)
                {
                    var _conference = new Conference
                    {
                        ConferenceId = 0,
                        ConferenceName = dto.ConferenceName,
                        Capacity = dto.Capacity,
                        IsActive = dto.IsActive,
                    };

                    await _context.Conferences.AddAsync(_conference);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>
                    {
                        Data = "Successfully added conference.",
                        ErrorMessage = "",
                        IsSuccess = true
                    };

                }
                else
                {
                    var _apiMessage = "";
                    var _updateConference = await _context.Conferences.FirstOrDefaultAsync(c => c.ConferenceId == dto.ConferenceId);

                    if (_updateConference != null)
                    {
                        _updateConference.ConferenceName = dto.ConferenceName;
                        _updateConference.Capacity = dto.Capacity;
                        _updateConference.IsActive = dto.IsActive;
                        _context.Conferences.Update(_updateConference);
                        await _context.SaveChangesAsync();
                        _apiMessage = "Successfully update conference.";
                    }
                    else
                    {
                        _apiMessage = "Conference does not exist or has been deleted";
                    }


                    return new ApiResponse<string>
                    {
                        Data = _apiMessage,
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
                    ErrorMessage = $"Error adding a conference: {ex.Message}  \r\n Inner Exception: {ex.InnerException.Message}",
                    IsSuccess = true
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteConference(long ID)
        {
            try
            {
                var _apiMessage = "";
                var _removeConference = await _context.Conferences.FirstOrDefaultAsync(c => c.ConferenceId == ID);

                if (_removeConference != null)
                {
                    _context.Conferences.Remove(_removeConference);
                    await _context.SaveChangesAsync();

                    _apiMessage = "Conference successfully deleted";
                }
                else
                {
                    _apiMessage = "Conference does not exist, or has been deleted";
                }

                return new ApiResponse<string>
                {
                    Data = _apiMessage,
                    ErrorMessage = "",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ApiResponse<List<ConferenceDto>>> GetAllConference()
        {
            try
            {
                var _conferences = await _context.Conferences.Select(c => new ConferenceDto
                {
                    ConferenceId = c.ConferenceId,
                    ConferenceName = c.ConferenceName,
                    Capacity = c.Capacity,
                    IsActive = c.IsActive,
                }).ToListAsync();

                return new ApiResponse<List<ConferenceDto>>
                {
                    Data = _conferences,
                    ErrorMessage = "",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<List<ConferenceDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error retrieving conference data: {ex.Message}",
                    IsSuccess = false
                };
            }
        }


        #endregion
    }
}
