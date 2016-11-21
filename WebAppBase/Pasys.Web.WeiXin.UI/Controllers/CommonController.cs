using Pasys.Web.Core;
using Pasys.Web.WeiXin.UI.Models;
using Pasys.Web.WeiXin.UI.Utility;
using SharedUtilitys.Helper;
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateCode(VerificationCodeViewModel model)
        {
            if (!ValidateHelper.IsMobile (model.Phone))
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
                bl = SMSes.Instance.SendSCVerifySMS(model.Phone, string.Format("{0}", mobile_code));
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
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