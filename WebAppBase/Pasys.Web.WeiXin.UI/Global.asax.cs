using Pasys.Web.Identity.Models;
using Pasys.Web.MemberCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pasys.Web.WeiXin.UI
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

            RegisterWeixinThreads();//激活微信缓存（必须）
            RegisterWeixinPay();//注册微信支付

            Senparc.Weixin.Config.IsDebug = false;//这里设为Debug状态时，/App_Data/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭

        }

        private void CreateDb()
        {
            AppIdentityDbContext.CreateForce();
            MemberCardDbContext.CreateForce();
        }
        /// <summary>
        /// 激活微信缓存
        /// </summary>
        private void RegisterWeixinThreads()
        {
            Senparc.Weixin.Threads.ThreadUtility.Register();
        }

        /// <summary>
        /// 注册微信支付
        /// </summary>
        private void RegisterWeixinPay()
        {
            //提供微信支付信息
            var WeiXinMPConfig = Pasys.Web.Core.ConfigManager.GetWeiXinMPConfigInfo();
            var weixinPay_PartnerId = WeiXinMPConfig.WeixinPay_PartnerId;
            var weixinPay_Key = WeiXinMPConfig.WeixinPay_Key;
            var weixinPay_AppId = WeiXinMPConfig.WeixinPay_AppId;
            var weixinPay_AppKey = WeiXinMPConfig.WeixinPay_AppKey;
            var weixinPay_TenpayNotify = WeiXinMPConfig.WeixinPay_TenpayNotify;

            var tenPayV3_MchId = WeiXinMPConfig.TenPayV3_MchId;
            var tenPayV3_Key = WeiXinMPConfig.TenPayV3_Key;
            var tenPayV3_AppId = WeiXinMPConfig.TenPayV3_AppId;
            var tenPayV3_AppSecret = WeiXinMPConfig.TenPayV3_AppSecret;
            var tenPayV3_TenpayNotify = WeiXinMPConfig.TenPayV3_TenpayNotify;

            var weixinPayInfo = new Senparc.Weixin.MP.TenPayLib.TenPayInfo(weixinPay_PartnerId, weixinPay_Key, weixinPay_AppId, weixinPay_AppKey, weixinPay_TenpayNotify);
            Senparc.Weixin.MP.TenPayLib.TenPayInfoCollection.Register(weixinPayInfo);
            var tenPayV3Info = new Senparc.Weixin.MP.TenPayLibV3.TenPayV3Info(tenPayV3_AppId, tenPayV3_AppSecret, tenPayV3_MchId, tenPayV3_Key,
                                                tenPayV3_TenpayNotify);
            Senparc.Weixin.MP.TenPayLibV3.TenPayV3InfoCollection.Register(tenPayV3Info);
        }

    }
}
