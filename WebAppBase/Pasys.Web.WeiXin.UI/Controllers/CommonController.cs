using Pasys.Web.WeiXin.UI.Models;
using Pasys.Web.WeiXin.UI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.WeiXin.UI.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        [HttpPost]
        public ActionResult GenerateCode(VerificationCodeViewModel model)
        {
            if (string.IsNullOrEmpty(model.Phone) || model.Phone.Length != 11)
            {
                var r = new { Successed = false, Message = "请检查手机号" };
                return Json(r);
            }

            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"1[34578]\d{9}");
            if (!reg.Match(model.Phone).Success)
            {
                var r = new { Successed = false, Message = "请检查手机号" };
                return Json(r);
            }

            Random rad = new Random();
            int mobile_code = rad.Next(1000, 10000);

            var validateInfo = new SMSValidateInfo()
            {
                ValidateType = 0,
                Phone = model.Phone,
                VerificationCode = string.Format("{0}", mobile_code)
            };

            bool bl = InsertValidateCode(validateInfo);
            if (!bl)
            {
                var r = new { Successed = false, Message = "发送失败" };
                return Json(r);
            }
            try
            {
                bl = SMSes.SendSCVerifySMS(model.Phone, string.Format("{0}", mobile_code));
                if (!bl)
                {
                    var r = new { Successed = false, Message = "发送失败" };
                    return Json(r);
                }
                else
                {
                    var r = new { Successed = true, Message = "OK" };
                    return Json(r);
                }
            }
            catch (Exception ex)
            {
                var r = new { Successed = false, Message = "发送失败" + ex.Message };
                return Json(r);
            }
        }

        [HttpPost]
        public ActionResult CheckVerificationCode(VerificationCodeViewModel model)
        {
            var validateInfo = new SMSValidateInfo()
            {
                ValidateType = 0,
                Phone = model.Phone,
                VerificationCode = string.Format("{0}", model.Code)
            };

            bool bl = CheckSMSValidate(validateInfo);
            if (!bl)
            {
                var r = new { Successed = false, Message = "验证失败" };
                return Json(r);
            }
            else
            {
                var r = new { Successed = true, Message = "验证成功" };
                return Json(r);
            }

        }

        private bool InsertValidateCode(SMSValidateInfo validateInfo)
        {
            throw new NotImplementedException();
        }

        private bool CheckSMSValidate(SMSValidateInfo validateInfo)
        {
            throw new NotImplementedException();
        }


    }
}