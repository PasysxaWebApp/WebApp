using Pasys.Web.Core;
using System;
using System.Text;


namespace Pasys.Web.Core
{
    /// <summary>
    /// 短信操作管理类
    /// </summary>
    public class SMSes
    {
        private ISMSStrategy smsStrategy = null;
        private GlobalConfigInfo globalConfig = null;
        private SMSConfigInfo smsConfig = null;

        private SMSes()
        {
            globalConfig = ConfigManager.GetGlobalConfig();
            smsConfig = ConfigManager.GetSMSConfigInfo();
            smsStrategy = StrategyManager.GetSMSStrategy();
        }

        private static SMSes _instance = new SMSes();
        public static SMSes Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 发送找回密码短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public bool SendFindPwdMobile(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(smsConfig.FindPwdBody);
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送验证手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public bool SendSCVerifySMS(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(smsConfig.SCVerifyBody);
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        private StringBuilder CreatePlaceholderStringBuilder(string configBody)
        {
            var body = new StringBuilder(configBody);
            body.Replace("{mallname}", globalConfig.SiteTitle);
            body.Replace("{regtime}", Pasys.Core.Helper.CommonHelper.GetDateTime());
            return body;
        }

        /// <summary>
        /// 安全中心发送确认更新手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public bool SendSCUpdateSMS(string to, string code)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(smsConfig.SCUpdateBody);
            body.Replace("{code}", code);
            return smsStrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 发送注册欢迎短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <returns></returns>
        public bool SendWebcomeSMS(string to)
        {
            StringBuilder body = CreatePlaceholderStringBuilder(smsConfig.WebcomeBody);
            body.Replace("{mobile}", to);
            return smsStrategy.Send(to, body.ToString());
        }

        public bool SendMessageSMS(string to, string message)
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
