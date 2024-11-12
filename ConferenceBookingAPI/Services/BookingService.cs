using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;
using ConferenceBookingAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConferenceBookingAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly ConferenceBookingContext _context;

        public BookingService(ConferenceBookingContext context)
        {
            _context = context;
        }

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


                    var startDate = dto.BookedDate;

                    if (dto.RecurringType == null)
                    {
                        // Create a single non-recurring booking
                        var booking = CreateBookingFromDto(dto, startDate);
                        await _context.Bookings.AddAsync(booking);
                    }
                    else
                    {
                        // Handle recurring bookings
                        int incrementDays = dto.RecurringType switch
                        {
                            "daily" => 1,
                            "weekly" => 7,
                            "monthly" => 0, // For monthly, we handle it separately
                            _ => 0
                        };

                        while (startDate <= dto.RecurringEndDate?.AddDays(1))
                        {

                            //if (startDate?.DayOfWeek == DayOfWeek.Sunday)
                            //{
                            //    continue;
                            //}


                            var booking = CreateBookingFromDto(dto, startDate);

                            await _context.Bookings.AddAsync(booking);

                            if (dto.RecurringType == "monthly")
                            {
                                startDate = startDate?.AddMonths(1);
                            }
                            else if (incrementDays > 0)
                            {
                                startDate = startDate?.DayOfWeek == DayOfWeek.Saturday ? startDate?.AddDays(incrementDays + 1) : startDate?.AddDays(incrementDays);
                            }
                        }
                    }


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
                    var _updateBooking = await _context.Bookings.Include(s => s.Status).FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);
                    var _statuses = await _context.Statuses.ToListAsync();


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
                        _updateBooking.ApprovedBy = dto.ApprovedBy ?? _updateBooking.ApprovedBy;
                        _updateBooking.Organizer = dto.Organizer ?? _updateBooking.Organizer;
                        _updateBooking.ExpectedAttendees = dto.ExpectedAttendees ?? _updateBooking.ExpectedAttendees;
                        _updateBooking.ContactNumber = dto.ContactNumber ?? _updateBooking.ContactNumber;
                        _updateBooking.Department = dto.Department ?? _updateBooking.Department;
                        _updateBooking.EmailAddress = dto.EmailAddress ?? _updateBooking.EmailAddress;
                        _updateBooking.ConferenceId = dto.ConferenceId ?? _updateBooking.ConferenceId;
                        _updateBooking.BookedDate = dto.BookedDate ?? _updateBooking.BookedDate;
                        _updateBooking.BookingStart = dto.BookingStart ?? _updateBooking.BookingStart;
                        _updateBooking.BookingEnd = dto.BookingEnd ?? _updateBooking.BookingEnd;
                        _updateBooking.Description = dto.Description ?? _updateBooking.Description;
                        _updateBooking.Purpose = dto.Purpose ?? _updateBooking.Purpose;


                        if (dto.StatusCode != null && dto.StatusCode == _statuses.Find(x => x.StatusName == "approved")!.StatusId)
                        {
                            var _conflictingBookings = await _context.Bookings
                                                                .Where(b =>
                                                                    b.BookedDate == dto.BookedDate &&
                                                                    (
                                                                        (dto.BookingStart >= b.BookingStart && dto.BookingStart <= b.BookingEnd) ||
                                                                        (dto.BookingEnd >= b.BookingStart && dto.BookingEnd <= b.BookingEnd) ||
                                                                        (dto.BookingStart <= b.BookingStart && dto.BookingEnd >= b.BookingEnd)
                                                                    )
                                                                ).ToListAsync();


                            if (_conflictingBookings != null)
                            {
                                var rejectStatus = _statuses.Find(x => x.StatusName == "rejected");
                                foreach(var booking in _conflictingBookings)
                                {
                                    booking.Status = rejectStatus;
                                    booking.StatusCode = rejectStatus!.StatusId;
                                    booking.Description = "Conference admin has approved a priority meeting";
                                }

                            }
                            _updateBooking.StatusCode = dto.StatusCode;

                        }
                        else
                        {
                            _updateBooking.StatusCode = dto.StatusCode ?? _updateBooking.StatusCode;
                        }

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
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Data = "",
                    ErrorMessage = $"Error: {ex.Message} | Inner Exception: {ex.InnerException.Message}",
                    IsSuccess = false
                };
            }
        }

        private Booking CreateBookingFromDto(BookingDto dto, DateOnly? date)
        {
            return new Booking
            {
                Organizer = dto.Organizer,
                ExpectedAttendees = dto.ExpectedAttendees,
                ContactNumber = dto.ContactNumber,
                Department = dto.Department,
                EmailAddress = dto.EmailAddress,
                ConferenceId = (int)dto.ConferenceId,
                BookedDate = (DateOnly)date!,
                BookingStart = (TimeOnly)dto.BookingStart,
                BookingEnd = (TimeOnly)dto.BookingEnd,
                Description = dto.Description,
                Purpose = dto.Purpose,
                StatusCode = 0,
                RecurringType = dto.RecurringType,
                RecurringEndDate = dto.RecurringEndDate,
            };

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
                var _booknings = await _context.Bookings.Include(s => s.Status).Select(b => new BookingDto
                {

                    BookingId = b.BookingId,
                    ApprovedBy = b.ApprovedBy,

                    //Organizer info
                    Organizer = b.Organizer,
                    ExpectedAttendees = b.ExpectedAttendees,
                    Department = b.Department,
                    ContactNumber = b.ContactNumber,
                    EmailAddress = b.EmailAddress,

                    //Booking Info
                    ConferenceId = b.ConferenceId,
                    BookedDate = b.BookedDate,
                    BookingStart = b.BookingStart,
                    BookingEnd = b.BookingEnd,
                    Description = b.Description,
                    Purpose = b.Purpose,
                    StatusCode = b.StatusCode,
                    StatusName = b.Status.StatusName
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

        public async Task<ApiResponse<BookingDto>> GetBookingByBookingId(long ID)
        {
            try
            {
                var _booking = await (from b in _context.Bookings
                                      where b.BookingId == ID
                                      select new BookingDto
                                      {
                                          BookingId = b.BookingId,
                                          ApprovedBy = b.ApprovedBy,

                                          // Organizer info
                                          Organizer = b.Organizer,
                                          ExpectedAttendees = b.ExpectedAttendees,
                                          Department = b.Department,
                                          ContactNumber = b.ContactNumber,
                                          EmailAddress = b.EmailAddress,

                                          // Booking Info
                                          ConferenceId = b.ConferenceId,
                                          BookedDate = b.BookedDate,
                                          BookingStart = b.BookingStart,
                                          BookingEnd = b.BookingEnd,
                                          Description = b.Description,
                                          Purpose = b.Purpose,
                                          StatusCode = b.StatusCode,

                                          // Status Info
                                          StatusName = b.Status != null ? b.Status.StatusName : null
                                      }).FirstOrDefaultAsync();

                if (_booking == null)
                {
                    return new ApiResponse<BookingDto>
                    {
                        Data = null,
                        ErrorMessage = "No Bookings found by that ID",
                        IsSuccess = false
                    };
                }


                return new ApiResponse<BookingDto>
                {
                    Data = _booking,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookingDto>
                {
                    Data = null,
                    ErrorMessage = $"Error retrieving booking: {ex.Message} ",
                    IsSuccess = false
                };

            }
        }

        public async Task<ApiResponse<List<BookingDto>>> GetBookingByConferenceId(long ID)
        {
            try
            {
                var _booking = await (from b in _context.Bookings
                                      where b.ConferenceId == ID
                                      select new BookingDto
                                      {
                                          BookingId = b.BookingId,
                                          ApprovedBy = b.ApprovedBy,

                                          //Organizer info
                                          Organizer = b.Organizer,
                                          ExpectedAttendees = b.ExpectedAttendees,
                                          Department = b.Department,
                                          ContactNumber = b.ContactNumber,
                                          EmailAddress = b.EmailAddress,

                                          //Booking Info
                                          ConferenceId = b.ConferenceId,
                                          BookedDate = b.BookedDate,
                                          BookingStart = b.BookingStart,
                                          BookingEnd = b.BookingEnd,
                                          Description = b.Description,
                                          Purpose = b.Purpose,
                                          StatusCode = b.StatusCode,
                                          StatusName = b.Status != null ? b.Status.StatusName : null
                                      }).ToListAsync();
                if (_booking == null)
                {
                    return new ApiResponse<List<BookingDto>>
                    {
                        Data = null,
                        ErrorMessage = "No Bookings found by that ID",
                        IsSuccess = false
                    };
                }


                return new ApiResponse<List<BookingDto>>
                {
                    Data = _booking,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookingDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error retrieving booking: {ex.Message} ",
                    IsSuccess = false
                };

            }
        }

        public async Task<ApiResponse<List<BookingDto>>> GetBookingByDate(DateOnly date)
        {
            try
            {
                var _booking = await (from b in _context.Bookings
                                      where b.BookedDate == date
                                      select new BookingDto
                                      {
                                          BookingId = b.BookingId,
                                          ConferenceId = b.ConferenceId,
                                          BookedDate = b.BookedDate,
                                          BookingStart = b.BookingStart,
                                          BookingEnd = b.BookingEnd,
                                          Description = b.Description,
                                          Purpose = b.Purpose,
                                          StatusCode = b.StatusCode,
                                          StatusName = b.Status.StatusName
                                      }).ToListAsync();
                if (_booking == null)
                {
                    return new ApiResponse<List<BookingDto>>
                    {
                        Data = null,
                        ErrorMessage = "No Bookings found by that date",
                        IsSuccess = false
                    };
                }


                return new ApiResponse<List<BookingDto>>
                {
                    Data = _booking,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BookingDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error retrieving booking: {ex.Message} ",
                    IsSuccess = false
                };

            }

        }


        #endregion

        #region Status Service


        public async Task<ApiResponse<string>> AddOrUpdateStatus(StatusDto dto)
        {
            try
            {
                var _successMessage = "";
                var _errorMessage = "";

                if (dto.StatusId == null)
                {
                    var _status = new Status
                    {
                        StatusName = dto.StatusName
                    };

                    await _context.Statuses.AddAsync(_status);
                    await _context.SaveChangesAsync();
                    _successMessage = "Successfully added status";



                }
                else
                {
                    var _existingStatus = await _context.Statuses.FirstOrDefaultAsync(s => s.StatusId == dto.StatusId);
                    if (_existingStatus == null)
                    {
                        _errorMessage = "Status does not exist";
                    }
                    else
                    {
                        _existingStatus.StatusName = dto.StatusName;
                        await _context.SaveChangesAsync();
                        _successMessage = "Successfully updated status";
                    }



                }

                return new ApiResponse<string>
                {
                    Data = _successMessage,
                    ErrorMessage = _errorMessage,
                    IsSuccess = string.IsNullOrEmpty(_errorMessage)
                };

            }
            catch (Exception ex)
            {

                return new ApiResponse<string>
                {
                    Data = "",
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = true
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteStatus(int id)
        {
            try
            {
                var _successMessage = "";
                var _errorMessage = "";
                var _existingStatus = await _context.Statuses.FirstOrDefaultAsync(s => s.StatusId == id);
                if (_existingStatus == null)
                {
                    _errorMessage = "Status does not exist";
                }
                else
                {
                    _context.Statuses.Remove(_existingStatus);
                    await _context.SaveChangesAsync();
                    _successMessage = "Deleted status!";
                }
                return new ApiResponse<string>
                {
                    Data = _successMessage,
                    ErrorMessage = _errorMessage,
                    IsSuccess = string.IsNullOrEmpty(_errorMessage)
                };

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

        public async Task<ApiResponse<List<StatusDto>>> GetStatuses()
        {
            try
            {
                var _statuses = await _context.Statuses.Select(s => new StatusDto
                {
                    StatusId = s.StatusId,
                    StatusName = s.StatusName
                }).ToListAsync();

                return new ApiResponse<List<StatusDto>>
                {
                    Data = _statuses,
                    ErrorMessage = "",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<StatusDto>>
                {
                    Data = [],
                    ErrorMessage = $"Error: {ex.Message}",
                    IsSuccess = true
                };
            }
        }


        #endregion
    }
}
