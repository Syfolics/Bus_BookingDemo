using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusBookingSystem.Models
{
    public class Bus
    {
        public int BusId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(9)]
        public string RegNo { get; set; }

        [Required]
        [Range(maximum: 72, minimum: 55)]
        public int NumSeats { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}