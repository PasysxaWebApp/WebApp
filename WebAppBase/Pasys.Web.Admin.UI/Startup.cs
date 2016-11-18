using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pasys.Web.Admin.UI.Startup))]
namespace Pasys.Web.Admin.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); 
        }
    }
}
