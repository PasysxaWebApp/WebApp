using Pasys.Web.Identity.Models;
using Pasys.Web.MemberCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pasys.Web.Admin.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CreateDb();

            //var globalConfig = Pasys.Web.Core.ConfigManager.GetGlobalConfig();
            //var rdbs= Pasys.Web.Core.ConfigManager.GetRDBSConfig();
            //var smsConfig = Pasys.Web.Core.ConfigManager.GetSMSConfigInfo();
            //var weixin = Pasys.Web.Core.ConfigManager.GetWeiXinMPConfigInfo();

            //var localConfig= Pasys.Web.Core.ConfigManager.GetConfig<Pasys.Web.Admin.UI.AppConfig.LocalConfigInfo>();
            //var localConfig2= Pasys.Web.Core.ConfigManager.GetConfig<Pasys.Web.Admin.UI.AppConfig.LocalConfig>();

            //var smsStrategy= Pasys.Web.Core.StrategyManager.GetSMSStrategy();
        }


        private void CreateDb()
        {
            AppIdentityDbContext.CreateForce();
            MemberCardDbContext.CreateForce();
        }
    }
}
