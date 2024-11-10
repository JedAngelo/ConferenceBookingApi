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
                            var _existingBooking = await _context.Bookings.FirstOrDefaultAsync(b =>
                                b.BookedDate == startDate &&
                                (
                                    (dto.BookingStart >= b.BookingStart && dto.BookingStart <= b.BookingEnd) ||
                                    (dto.BookingEnd >= b.BookingStart && dto.BookingEnd <= b.BookingEnd) ||
                                    (dto.BookingStart <= b.BookingStart && dto.BookingEnd >= b.BookingEnd)
                                )
                            );          
                            var booking = CreateBookingFromDto(dto, startDate);

                            await _context.Bookings.AddAsync(booking);

                            if (dto.RecurringType == "monthly")
                            {
                                startDate = startDate?.AddMonths(1);
                            }
                            else if (incrementDays > 0)
                            {
                                startDate = startDate?.AddDays(incrementDays);
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

                        if (dto.Status != null && dto.Status == "approved")
                        {
                            var _conflicBooking = await _context.Bookings.FirstOrDefaultAsync(b =>
                                b.BookedDate == dto.BookedDate &&
                                (
                                    (dto.BookingStart >= b.BookingStart && dto.BookingStart <= b.BookingEnd) ||
                                    (dto.BookingEnd >= b.BookingStart && dto.BookingEnd <= b.BookingEnd) ||
                                    (dto.BookingStart <= b.BookingStart && dto.BookingEnd >= b.BookingEnd)
                                )
                            );

                            if (_conflicBooking != null)
                            {
                                _conflicBooking.Status = "rejected";
                                _conflicBooking.Description = "Conference admin has approved a priority meeting";
                            }
                            _updateBooking.Status = dto.Status;
                            
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
                Status = "pending",
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
                var _booknings = await _context.Bookings.Select(b => new BookingDto
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
                                          Status = b.Status,
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
                                          Status = b.Status,
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

        public async Task<ApiResponse<string>> UpdateBookingStatus(UpdateBookingStatusDto dto)
        {
            try
            {
                var _apiMessage = "";
                var _updateBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);

                if (_updateBooking != null)
                {
                    _updateBooking.Status = dto.Status;
                    if (dto.ApprovedBy != null)
                    {
                        _updateBooking.ApprovedBy = dto.ApprovedBy;
                    }
                    _context.Bookings.Update(_updateBooking);
                    await _context.SaveChangesAsync();
                    _apiMessage = "Successfully updated booking information.";
                }
                else
                {
                    _apiMessage = "Booking does not exist!";
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
                    ErrorMessage = $"ERROR: {ex.Message}",
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
                                          Status = b.Status,
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
    }
}
