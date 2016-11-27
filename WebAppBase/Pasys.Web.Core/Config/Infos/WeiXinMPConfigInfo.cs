using Pasys.Core.ConfigInfo;
using System;

namespace Pasys.Web.Core
{
    /// <summary>
    /// 微信配置信息类
    /// </summary>
    [Serializable]
    public class WeiXinMPConfigInfo : IConfigInfo
    {
        public string Ver { get; set; }

        public string WeixinToken { get; set; }
        public string WeixinEncodingAESKey { get; set; }
        public string WeixinAppId { get; set; }
        public string WeixinAppSecret { get; set; }
        public string WeixinAgentUrl { get; set; }
        public string WeixinAgentToken { get; set; }
        public string WeixinAgentWeiweihiKey { get; set; }
        public string WeixinPay_Tenpay { get; set; }
        public string WeixinPay_PartnerId { get; set; }
        public string WeixinPay_Key { get; set; }
        public string WeixinPay_AppId { get; set; }
        public string WeixinPay_AppKey { get; set; }
        public string WeixinPay_TenpayNotify { get; set; }
        public string TenPayV3_MchId { get; set; }
        public string TenPayV3_Key { get; set; }
        public string TenPayV3_AppId { get; set; }
        public string TenPayV3_AppSecret { get; set; }
        public string TenPayV3_TenpayNotify { get; set; }
        public string Component_Appid { get; set; }
        public string Component_Secret { get; set; }
        public string Component_Token { get; set; }
        public string Component_EncodingAESKey { get; set; }
        public string AuthorizeState { get; set; }
    }
}
