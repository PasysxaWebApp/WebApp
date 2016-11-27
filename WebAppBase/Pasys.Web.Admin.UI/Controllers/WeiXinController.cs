using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class WeiXinController : WorkController
    {
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        public ActionResult MPRequest(PostModel postModel, string echostr)
        {
            var config = WorkContext.WeiXinMPConfig;
            var Token = config.WeixinToken;
            if (Senparc.Weixin.MP.CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        [HttpPost]
        public ActionResult MPRequest(PostModel postModel)
        {
            var config = WorkContext.WeiXinMPConfig;
            var Token = config.WeixinToken;
            if (!Senparc.Weixin.MP.CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = config.WeixinEncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = config.WeixinAppId;//根据自己后台的设置保持一致

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new MessageHandler.MessageHandler(Request.InputStream, postModel, maxRecordCount);
            try
            {
                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //执行微信处理过程
                messageHandler.Execute();

                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                var rsp = messageHandler.ResponseMessage;
                if (rsp == null || rsp is ResponseMessageNoResponse)
                {
                    return Content("success");
                }
                return new WeixinResult(messageHandler);//v0.8+
            }
            catch (Exception)
            {
                return Content("success");
            }
        }
    }
}