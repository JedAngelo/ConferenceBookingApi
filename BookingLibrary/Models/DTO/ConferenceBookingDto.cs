using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingLibrary.Models.DTO
{
    public class ConferenceBookingDto
    {
        public Guid ConferenceID { get; set; }
        public DateTime Schedule { get; set; }
        public string Meeting { get; set; }
    }
}
