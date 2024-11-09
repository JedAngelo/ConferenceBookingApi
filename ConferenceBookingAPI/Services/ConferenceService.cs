using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;
using ConferenceBookingAPI.UserAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingAPI.Services
{
    public class ConferenceService : IConferenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConferenceService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        #region Conference Services

        public async Task<ApiResponse<string>> AddOrUpdateConference(ConferenceDto dto)
        {
            try
            {

                if (dto.ConferenceId == null)
                {
                    var _existingConference = await _context.Conferences.AnyAsync(x => x.ConferenceName == dto.ConferenceName);
                    if (_existingConference)
                    {
                        return new ApiResponse<string>
                        {
                            Data = "",
                            ErrorMessage = "A conference with this name already exists.",
                            IsSuccess = false
                        };
                    }

                    var _conference = new Conference
                    {   
                        ConferenceName = dto.ConferenceName,
                        Capacity = dto.Capacity,
                        IsActive = dto.IsActive,
                        ApplicationUser = new List<ApplicationUser>()
                    };
                                                      


                    if(dto.UserDtos != null)
                    {
                        foreach(var users in dto.UserDtos)
                        {
                            var userExist = await _userManager.FindByIdAsync(users.UserId);
                            if (userExist != null)
                            {
                                userExist.ConferenceId = dto.ConferenceId;
                                _conference.ApplicationUser.Add(userExist);
                            }
                        }
                    }

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
                        //_context.Conferences.Update(_updateConference);

                        if (dto.UserDtos != null)
                        {
                            if (_updateConference.ApplicationUser == null)
                            {
                                _updateConference.ApplicationUser = new List<ApplicationUser>();
                            }
                            foreach (var users in dto.UserDtos)
                            {
                                var userExist = await _userManager.FindByIdAsync(users.UserId);
                                if (userExist != null)
                                {
                                    userExist.ConferenceId = dto.ConferenceId;
                                    _updateConference.ApplicationUser.Add(userExist);
                                }
                            }
                        }


                        await _context.SaveChangesAsync();
                        _apiMessage = "Successfully updated conference.";
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
                var _conferences = await _context.Conferences.Include(a => a.ApplicationUser).Select(c => new ConferenceDto
                {
                    ConferenceId = c.ConferenceId,
                    ConferenceName = c.ConferenceName,
                    Capacity = c.Capacity,
                    IsActive = c.IsActive,
                    UserDtos = c.ApplicationUser!.Select(x => new UsersDto
                    {
                        UserId = x.Id,
                        UserName = x.UserName!,
                        ConferenceId = x.ConferenceId
                    }).ToList()
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

        public async Task<ApiResponse<ConferenceDto>> GetConferenceById(int ID)
        {
            try
            {
                var _conferences = await _context.Conferences.Where(x => x.ConferenceId == ID).Include(a => a.ApplicationUser).Select(c => new ConferenceDto
                {
                    ConferenceId = c.ConferenceId,
                    ConferenceName = c.ConferenceName,
                    Capacity = c.Capacity,
                    IsActive = c.IsActive,
                    UserDtos = c.ApplicationUser!.Select(x => new UsersDto
                    {
                        UserId = x.Id,
                        UserName = x.UserName!,
                        ConferenceId = x.ConferenceId
                    }).ToList()
                }).FirstOrDefaultAsync();

                return new ApiResponse<ConferenceDto>
                {
                    Data = _conferences,
                    ErrorMessage = "",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<ConferenceDto>
                {
                    Data = new ConferenceDto(),
                    ErrorMessage = $"Error retrieving conference data: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
        #endregion
    }
}
