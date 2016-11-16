using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebAppBase
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var globalConfig = Pasys.Web.Core.ConfigManager.GetGlobalConfig();
            var rdbs= Pasys.Web.Core.ConfigManager.GetRDBSConfig();
            var smsConfig = Pasys.Web.Core.ConfigManager.GetSMSConfigInfo();
            var weixin = Pasys.Web.Core.ConfigManager.GetWeiXinMPConfigInfo();

            var localConfig= Pasys.Web.Core.ConfigManager.GetConfig<WebAppBase.AppConfig.LocalConfigInfo>();
            var localConfig2= Pasys.Web.Core.ConfigManager.GetConfig<WebAppBase.AppConfig.LocalConfig>();

            var smsStrategy= Pasys.Web.Core.StrategyManager.GetSMSStrategy();
        }
    }
}
