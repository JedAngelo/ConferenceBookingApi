using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingLibrary.Models
{
    public class ConferenceBooking
    {
        [Key]
        public Guid ConferenceID{ get; set; }
        public DateTime Schedule { get; set; }
        public string Meeting { get; set; }

    }
}
