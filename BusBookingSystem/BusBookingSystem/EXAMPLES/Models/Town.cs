using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusBookingSystem.Models
{
    public class Town
    {
        public int TownId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}