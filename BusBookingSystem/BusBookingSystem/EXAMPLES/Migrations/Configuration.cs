namespace BusBookingSystem.Migrations
{
    using BusBookingSystem.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BusBookingSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BusBookingSystem.Models.ApplicationDbContext";
        }

        public bool FindRole(string role, BusBookingSystem.Models.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            var r = rm.FindByNameAsync(role);
            if (r != null)
            {
                return true;
            }
            return false;
        }

        public bool FindUser(string userName, BusBookingSystem.Models.ApplicationDbContext context)
        {
            var um = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(context));
            var user = um.FindByName(userName);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public void AddRole(BusBookingSystem.Models.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            rm.Create(new IdentityRole("Admin"));
        }

        public void AddUser(BusBookingSystem.Models.ApplicationDbContext context)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            um.UserValidator = new UserValidator<ApplicationUser>(um) { AllowOnlyAlphanumericUserNames = false };
            var user = new ApplicationUser
            {
                UserName = ConfigurationManager.AppSettings["AdminEmail"].ToString(), 
                IdentityNo = "9310175785082", 
                PhoneNumber = "0123456789", 
                PhysicalAddress = "Blah blah blah",
                Email = ConfigurationManager.AppSettings["AdminEmail"].ToString(),
                LastName = "Syfo",
                FirstName = "Mthembu"
            };
            um.Create(user, "Password*1");
        }

        public void AssignUserToRole(BusBookingSystem.Models.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!rm.RoleExists("Admin"))
            {
                rm.Create(new IdentityRole("Admin"));
            }

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = um.FindByName(ConfigurationManager.AppSettings["AdminEmail"].ToString());

            um.AddToRole(user.Id, "Admin");
            um.Update(user);
        }

        protected override void Seed(BusBookingSystem.Models.ApplicationDbContext context)
        {
            if (FindRole("Admin", context) == false)
            {
                AddRole(context);
            }

            if (FindUser(ConfigurationManager.AppSettings["AdminEmail"].ToString(), context) == false)
            {
                AddUser(context);
            }
            AssignUserToRole(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
