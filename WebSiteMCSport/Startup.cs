using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSiteMCSport.Startup))]
namespace WebSiteMCSport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
