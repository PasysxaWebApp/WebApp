using Pasys.Web.Identity.Models;
using Pasys.Web.MemberCard;
using Pasys.Web.WeiXin;
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

            //ModelMetadataProviders.Current = new Pasys.Core.Web.MetadataProvider.EasyModelMetaDataProvider();

            CreateDb();

            //var globalConfig = Pasys.Web.Core.ConfigManager.GetGlobalConfig();
            //var rdbs= Pasys.Web.Core.ConfigManager.GetRDBSConfig();
            //var smsConfig = Pasys.Web.Core.ConfigManager.GetSMSConfigInfo();
            //var weixin = Pasys.Web.Core.ConfigManager.GetWeiXinMPConfigInfo();

            //var localConfig= Pasys.Web.Core.ConfigManager.GetConfig<Pasys.Web.Admin.UI.AppConfig.LocalConfigInfo>();
            //var localConfig2= Pasys.Web.Core.ConfigManager.GetConfig<Pasys.Web.Admin.UI.AppConfig.LocalConfig>();

            //var smsStrategy= Pasys.Web.Core.StrategyManager.GetSMSStrategy();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //RouteConfig.RegisterRoutes(routes);
        } 

        private void CreateDb()
        {
            /*             
                drop table [dbo].[__MigrationHistory];
                drop table [dbo].[account_m_rolefunctions];
                drop table [dbo].[account_m_userroles];
                drop table [dbo].[account_m_userclaims];
                drop table [dbo].[account_m_userlogins];
                drop table [dbo].[account_m_users];
                drop table [dbo].[account_m_funtions];
                drop table [dbo].[account_m_roles];
                drop table [dbo].[account_m_organizations];

                drop table [dbo].[mc_t_consumptions];
                drop table [dbo].[mc_m_membercards];

                drop table [dbo].[weixin_m_userbinds];
                drop table [dbo].[weixin_m_userinfos];
                drop table [dbo].[weixin_m_mpmenuinfos];
                drop table [dbo].[weixin_m_mps];
				drop table [dbo].[weixin_t_request_logs];
				drop table [dbo].[weixin_t_response_articles];
				drop table [dbo].[weixin_t_response_customerserviceaccounts];
				drop table [dbo].[weixin_t_request_rules];
				drop table [dbo].[weixin_t_responses];
             
             */
            AppIdentityDbContext.CreateForce();
            MemberCardDbContext.CreateForce();
            WeiXinDbContext.CreateForce();
        }
    }
}
