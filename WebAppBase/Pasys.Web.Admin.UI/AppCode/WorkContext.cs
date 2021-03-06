﻿using Pasys.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pasys.Web.Identity.Models;

namespace Pasys.Web.Admin.UI
{
    public class WorkContext:IWorkContext
    {
        public static  GlobalConfigInfo GLOBALCONFIG;//全局配置信息
        public static WeiXinMPConfigInfo WEIXINMP_CONFIG;
        public static SMSConfigInfo SMS_CONFIG;

        static WorkContext()
        {
            GLOBALCONFIG = Pasys.Web.Core.ConfigManager.GetGlobalConfig();
            WEIXINMP_CONFIG = Pasys.Web.Core.ConfigManager.GetWeiXinMPConfigInfo();
            SMS_CONFIG = Pasys.Web.Core.ConfigManager.GetSMSConfigInfo();
        }

        public string Area  //注册区域
        {
            get
            {
                return GLOBALCONFIG.WorkContextArea;
            }
        }

        //全局配置信息
        public GlobalConfigInfo GlobalConfig
        {
            get {
                return GLOBALCONFIG;
            }
        }
        public WeiXinMPConfigInfo WeiXinMPConfig
        {
            get
            {
                return WEIXINMP_CONFIG;
            }
        }
        public SMSConfigInfo SMSConfig
        {
            get
            {
                return SMS_CONFIG;
            }
        }

        public bool IsHttpAjax;//当前请求是否为ajax请求

        public string IP;//用户ip

        public ApplicationUser UserInfo;

        public string Url;//当前url

        public string UrlReferrer;//上一次访问的url
        
        public string UserId = "";//用户id

        public string UserName;//用户名

        public string Controller;//控制器

        public string Action;//动作方法

        public string PageKey;//页面标示符

        public WorkContext()
        {
        }
    }
}
