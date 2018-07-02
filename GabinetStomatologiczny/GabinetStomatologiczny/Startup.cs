using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GabinetStomatologiczny.Startup))]
namespace GabinetStomatologiczny
{
    public partial class Startup
    {
        // tedt
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
