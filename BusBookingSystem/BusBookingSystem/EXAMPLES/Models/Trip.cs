using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BusBookingSystem.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        [DataType(DataType.Date)]
        public DateTime TripDate { get; set; }

        public int TownId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string Departure { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string Arrival { get; set; }

        public int BusId { get; set; }

        [Display(Name = "No. seats available")]
        public int SeatsAvailable { get; set; }

        public decimal Fare { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Town Town { get; set; }
        public virtual Bus Bus { get; set; }

        public void setSeats(int busid)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var bus = db.Buses.Find(busid);

            this.SeatsAvailable = bus.NumSeats;
        }

        public async Task<bool> MakeBooking(int id, string un)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Trip trip = db.Trips.Find(id);

            if (trip != null)
            {
                if (trip.SeatsAvailable > 0)
                {
                    Booking booking = new Booking
                    {
                        BookingDate = DateTime.Now,
                        TripId = trip.TripId,
                        UserName = un
                    };
                    trip.SeatsAvailable--;
                    db.Bookings.Add(booking);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}