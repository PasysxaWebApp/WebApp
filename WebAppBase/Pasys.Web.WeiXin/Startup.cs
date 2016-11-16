using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pasys.Web.WeiXin.Startup))]
namespace Pasys.Web.WeiXin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
