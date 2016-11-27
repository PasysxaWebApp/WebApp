using Pasys.Core.ConfigInfo;
using System;

namespace Pasys.Web.Core
{
    /// <summary>
    /// 短信配置信息类
    /// </summary>
    [Serializable]
    public class SMSConfigInfo : IConfigInfo
    {
        public string Ver { get; set; }

        private string _url;//短信服务器地址
        private string _username;//短信账号
        private string _password;//短信密码
        private string _findpwdbody;//找回密码内容
        private string _scverifybody;//安全中心验证手机内容
        private string _scupdatebody;//安全中心确认更新手机内容
        private string _webcomebody;//注册欢迎信息
        private string _assignorderbody;//派单信息
        private string _assignorderworkerbody;//给洗车工的派单信息
        private string _carwashfinishedbody;//清洗完毕信息
        private string _submitOrderToCustomeBody;  //提交订单信息发消息
        private string _submitOrderToShopBody; //提交订单信息发消息
        private string _assignordershopworkerbody; //派单给商城飞毛腿
        /// <summary>
        /// 短信服务器地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 短信账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 找回密码内容
        /// </summary>
        public string FindPwdBody
        {
            get { return _findpwdbody; }
            set { _findpwdbody = value; }
        }

        /// <summary>
        /// 安全中心验证手机内容
        /// </summary>
        public string SCVerifyBody
        {
            get { return _scverifybody; }
            set { _scverifybody = value; }
        }

        /// <summary>
        /// 安全中心确认更新手机内容
        /// </summary>
        public string SCUpdateBody
        {
            get { return _scupdatebody; }
            set { _scupdatebody = value; }
        }

        /// <summary>
        /// 注册欢迎信息
        /// </summary>
        public string WebcomeBody
        {
            get { return _webcomebody; }
            set { _webcomebody = value; }
        }
        /// <summary>
        /// 派单信息
        /// </summary>
        public string AssignOrderBody
        {
            get { return _assignorderbody; }
            set { _assignorderbody = value; }
        }

        /// <summary>
        /// 给洗车工发送的派单信息
        /// </summary>
        public string AssignOrderWorkerBody
        {
            get { return _assignorderworkerbody; }
            set { _assignorderworkerbody = value; }
        }

        /// <summary>
        /// 清洗完毕信息
        /// </summary>
        public string CarWashFinishedBody
        {
            get { return _carwashfinishedbody; }
            set { _carwashfinishedbody = value; }
        }

        ///////////////////////////////XMJ start//////////////////////////////
        /// <summary>
        /// 订单信息
        /// </summary>
        public string SubmitOrderToCustomeBody
        {
            get { return _submitOrderToCustomeBody; }
            set { _submitOrderToCustomeBody = value; }
        }

        ///////////////////////////////XMJ end//////////////////////////////

        public string SubmitOrderToShopBody
        {
            get { return _submitOrderToShopBody; }
            set { _submitOrderToShopBody = value; }
        }
        /// <summary>
        /// 派单给商城飞毛腿
        /// </summary>
        public string AssignOrderShopWorkerBody
        {
            get { return _assignordershopworkerbody; }
            set { _assignordershopworkerbody = value; }
        }

    }
}
