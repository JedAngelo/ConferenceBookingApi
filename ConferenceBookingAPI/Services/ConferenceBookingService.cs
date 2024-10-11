using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ConferenceBookingAPI.Services
{
    public class ConferenceBookingService : IConferenceBookingService
    {
        private readonly ConferenceBookingContext _context;

        public ConferenceBookingService(ConferenceBookingContext context)
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


        #region Booking Services

        public async Task<ApiResponse<string>> AddOrUpdateBooking(BookingDto dto)
        {
            try
            {
                if (dto.BookingId == null)
                {
                    var _conferenceExist = await _context.Conferences.AnyAsync(c => c.ConferenceId == dto.ConferenceId);
                    if (!_conferenceExist)
                    {
                        return new ApiResponse<string>
                        {
                            Data = "",
                            ErrorMessage = "Error: Conference ID does not exist.",
                            IsSuccess = false
                        };
                    }

                    var _booking = new Booking
                    {
                        BookingId = 0,
                        ApprovedBy = dto.ApprovedBy,

                        //Organizer information
                        Organizer = dto.Organizer,
                        ExpectedAttendess = dto.ExpectedAttendess,
                        ContactNumber = dto.ContactNumber,
                        Department = dto.Department,
                        EmailAddress = dto.EmailAddress,


                        //Booking information
                        ConferenceId = dto.ConferenceId,
                        BookingStart = dto.BookingStart,
                        BookingEnd = dto.BookingEnd,
                        Description = dto.Description,
                        Purpose = dto.Purpose,
                        Status = dto.Status,
                    };

                    await _context.Bookings.AddAsync(_booking);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>
                    {
                        Data = "Successfully booked.",
                        ErrorMessage = "",
                        IsSuccess = true
                    };
                }
                else
                {


                    var _apiMessage = "";
                    var _updateBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);


                    var _conferenceExist = await _context.Conferences.AnyAsync(c => c.ConferenceId == dto.ConferenceId);
                    if (!_conferenceExist)
                    {
                        return new ApiResponse<string>
                        {
                            Data = "",
                            ErrorMessage = "Error: Conference ID does not exist.",
                            IsSuccess = false
                        };
                    }

                    if (_updateBooking != null)
                    {
                        _updateBooking.ApprovedBy = dto.ApprovedBy;

                        //Updated organizer informatnion
                        _updateBooking.Organizer = dto.Organizer;
                        _updateBooking.ExpectedAttendess = dto.ExpectedAttendess;
                        _updateBooking.ContactNumber = dto.ContactNumber;
                        _updateBooking.Department = dto.Department;
                        _updateBooking.EmailAddress = dto.EmailAddress;

                        //Updated booking info
                        _updateBooking.ConferenceId = dto.ConferenceId;
                        _updateBooking.BookingStart = dto.BookingStart;
                        _updateBooking.BookingEnd = dto.BookingEnd;
                        _updateBooking.Description = dto.Description;
                        _updateBooking.Purpose = dto.Purpose;
                        _updateBooking.Status = dto.Status;

                        _context.Bookings.Update(_updateBooking);
                        await _context.SaveChangesAsync();
                        _apiMessage = "Successfully updated booking information.";
                    }
                    else
                    {
                        _apiMessage = "Booking does not exist or has already been deleted.";
                    }

                    return new ApiResponse<string>
                    {
                        Data = _apiMessage,
                        ErrorMessage = "",
                        IsSuccess = false
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

        public async Task<ApiResponse<string>> DeleteBooking(long ID)
        {
            try
            {
                var _apiMessage = "";
                var _deleteBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == ID);

                if (_deleteBooking != null)
                {
                    _context.Bookings.Remove(_deleteBooking);
                    await _context.SaveChangesAsync();

                    _apiMessage = "Booking successfully removed.";
                }
                else
                {
                    _apiMessage = "Booking does not exist or has already been deleted.";
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

                return new ApiResponse<string>
                {
                    Data = "",
                    ErrorMessage = $"Error deleteing booking data: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ApiResponse<List<BookingDto>>> GetAllBookings()
        {
            try
            {
                var _booknings = await _context.Bookings.Select(b => new BookingDto
                {

                    BookingId = b.BookingId,
                    ApprovedBy = b.ApprovedBy,

                    //Organizer info
                    Organizer = b.Organizer,
                    ExpectedAttendess = b.ExpectedAttendess,
                    Department = b.Department,
                    ContactNumber = b.ContactNumber,
                    EmailAddress = b.EmailAddress,

                    //Booking Info
                    ConferenceId = b.ConferenceId,
                    BookingStart = b.BookingStart,
                    BookingEnd = b.BookingEnd,
                    Description = b.Description,
                    Purpose = b.Purpose,
                    Status = b.Status,

                }).ToListAsync();

                return new ApiResponse<List<BookingDto>>
                {
                    Data = _booknings,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookingDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error retrieving booking data: {ex.Message}",
                    IsSuccess = false
                };
            }
        }


        #endregion
    }
}
