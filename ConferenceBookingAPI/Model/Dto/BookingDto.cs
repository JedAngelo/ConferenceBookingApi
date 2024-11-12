﻿using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Model.Dto;

namespace ConferenceBookingAPI.Models.Dto
{
    public class BookingDto
    {
        public int? BookingId { get; set; }

        public string? ApprovedBy { get; set; } = null!;

        public DateOnly? BookedDate { get; set; }

        public TimeOnly? BookingStart { get; set; }

        public TimeOnly? BookingEnd { get; set; }

        public string? Organizer { get; set; }  

        public string? Department { get; set; }

        public string? ContactNumber { get; set; }

        public string? EmailAddress { get; set; }

        public int? ExpectedAttendees { get; set; }

        public string? Purpose { get; set; } = null!;

        public string? Description { get; set; }

        public int? StatusCode { get; set; } = 0;

        public string? RecurringType { get; set; }

        public DateOnly? RecurringEndDate { get; set; }

        public int? ConferenceId { get; set; }

        public string? StatusName { get; set; }

        //public virtual Conference? Conference { get; set; } = null!;
    }
}
