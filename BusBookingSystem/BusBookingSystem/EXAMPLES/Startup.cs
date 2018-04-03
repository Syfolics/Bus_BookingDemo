using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusBookingSystem.Startup))]
namespace BusBookingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
