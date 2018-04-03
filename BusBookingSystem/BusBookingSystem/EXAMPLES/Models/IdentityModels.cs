using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using BusBookingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BusBookingSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string IdentityNo { get; set; }

        public string PhysicalAddress { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Bus> Buses { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
    }
}