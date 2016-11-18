using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pasys.Web.WeiXin.UI.Startup))]
namespace Pasys.Web.WeiXin.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
