using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingAPI.Model;

public partial class ConferenceBookingContext : DbContext
{
    public ConferenceBookingContext()
    {
    }

    public ConferenceBookingContext(DbContextOptions<ConferenceBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Conference> Conferences { get; set; }

    
}
