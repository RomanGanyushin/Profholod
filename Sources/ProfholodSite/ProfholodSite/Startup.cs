using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProfholodSite.Startup))]
namespace ProfholodSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
