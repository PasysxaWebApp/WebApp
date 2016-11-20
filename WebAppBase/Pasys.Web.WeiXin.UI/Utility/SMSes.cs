using Pasys.Web.Core;
using System;
using System.Text;


namespace Pasys.Web.WeiXin.UI.Utility
{
    /// <summary>
    /// 短信操作管理类
    /// </summary>
    public partial class SMSes
    {
        private static ISMSStrategy smsStrategy = null;
        static SMSes()
        {
            smsStrategy = Pasys.Web.Core.StrategyManager.GetSMSStrategy();
        }
        /// <summary>
        /// 发送找回密码短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendFindPwdMobile(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(WorkContext.SMS_CONFIG.FindPwdBody);
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送验证手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCVerifySMS(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(WorkContext.SMS_CONFIG.SCVerifyBody);            
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        private static StringBuilder CreatePlaceholderStringBuilder(string configBody)
        {
            var body = new StringBuilder(configBody);          
            body.Replace("{mallname}", WorkContext.GLOBAL_CONFIG.SiteTitle);
            body.Replace("{regtime}", SharedUtilitys.Helper.CommonHelper.GetDateTime());
            return body;
        }

        /// <summary>
        /// 安全中心发送确认更新手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCUpdateSMS(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(WorkContext.SMS_CONFIG.SCUpdateBody);
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 发送注册欢迎短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <returns></returns>
        public static bool SendWebcomeSMS(string to)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(WorkContext.SMS_CONFIG.WebcomeBody);
            body.Replace("{mobile}", to);
            return smsStrategy.Send(to, body.ToString());
        }

        public static bool SendMessageSMS(string to, string message)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(message);
            if (body.Length > 0)
            {
                return smsStrategy.Send(to, body.ToString());
            }
            else
            {
                return false;
            }
        }
    }
}
