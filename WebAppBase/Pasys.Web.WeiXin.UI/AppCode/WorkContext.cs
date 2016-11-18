using Pasys.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pasys.Web.Identity.Models;

namespace Pasys.Web.WeiXin.UI
{
    public class WorkContext:IWorkContext
    {
        public GlobalConfigInfo GlobalConfig;//全局配置信息

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
           GlobalConfig= Pasys.Web.Core.ConfigManager.GetGlobalConfig();
        }
    }
}
