using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineKladilnica.Startup))]
namespace OnlineKladilnica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
