using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ConferenceBookingAPI.Model;

public partial class Booking
{
    public int BookingId { get; set; }

    public string ApprovedBy { get; set; } = null!;

    //public DateTime? BookingStart { get; set; }

    //public DateTime? BookingEnd { get; set; }


    public DateOnly BookedDate { get; set; }

    public TimeOnly BookingStart { get; set; }

    public TimeOnly BookingEnd { get; set; }

    public string? Organizer {  get; set; }

    public string? Department { get; set; }

    public string? ContactNumber { get; set; }

    public string? EmailAddress { get; set; }

    public int? ExpectedAttendees { get; set; }

    public string Purpose { get; set; } = null!;

    public string? Description { get; set; }

    [DefaultValue("pending")]
    public string? Status { get; set; } = "pending";

    public string? RecurringType { get; set; }

    public string? RecurringEndDate { get; set; }

    public int ConferenceId { get; set; }

    public virtual Conference Conference { get; set; } = null!;
}
