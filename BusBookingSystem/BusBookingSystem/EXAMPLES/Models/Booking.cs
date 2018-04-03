using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public int TripId { get; set; }
        public string UserName { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Trip Trip { get; set; }

        public async Task ConfirmBooking(Booking booking, string to)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            StringBuilder bld = new StringBuilder();
            string from = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            MailMessage message = new MailMessage(new MailAddress(from), new MailAddress(to));
            message.Subject = "Booking report";

            bld.Append("Hi, your booking was successful. Please review booking details below. <br/>");
            bld.Append("Id: " + booking.BookingId + "</br/>");
            bld.Append("Date of booking: " + booking.BookingDate + "</br/>");
            bld.Append("Date of trip: " + booking.Trip.TripDate + "</br/>");
            bld.Append("Trip destination: " + booking.Trip.Town.Name + "</br/>");
            bld.Append("Bus reg no: " + booking.Trip.Bus.RegNo + "</br/>");
            bld.Append("Time of departure: " + booking.Trip.Departure + "</br/>");
            bld.Append("Time of destination arrival: " + booking.Trip.Arrival + "</br/>");
            bld.Append("Total fare due: " + booking.Trip.Fare + "</br/>");
            message.Body = bld.ToString();
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient 
            { 
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["AdminEmail"].ToString(), ConfigurationManager.AppSettings["AdminPass"].ToString()),
                EnableSsl = true
            };
            await client.SendMailAsync(message);
        }
    }
}