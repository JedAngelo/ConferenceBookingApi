using System;
using System.Collections.Generic;

namespace ConferenceBookingAPI.Model;

public partial class Conference
{
    public int ConferenceId { get; set; }

    public string? ConferenceName { get; set; }

    public int? Capacity { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
