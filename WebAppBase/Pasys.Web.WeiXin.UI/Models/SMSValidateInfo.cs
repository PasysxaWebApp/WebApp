using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pasys.Web.WeiXin.UI.Models
{
    public class SMSValidateInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int SMSID { get; set; }
        /// <summary>
        /// 0 注册验证码
        /// 1 找回密码
        /// 2 更换手机
        /// </summary>
        public int ValidateType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; set; }

    }
}