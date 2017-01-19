using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdunbiKiddies.Startup))]
namespace AdunbiKiddies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
