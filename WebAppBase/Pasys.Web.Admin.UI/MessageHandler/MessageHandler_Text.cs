using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Pasys.Core.EntityManager;

namespace Pasys.Web.Admin.UI.MessageHandler
{
    public partial class MessageHandler : MessageHandler<MessageContext>
    {
        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            if (string.IsNullOrWhiteSpace(requestMessage.Content))
            {
                return new ResponseMessageNoResponse();
            }

            var ruleMng = new Pasys.Web.WeiXin.RequestRuleManager();
            var rules = ruleMng.FindRules(requestMessage.Content);
            if (rules == null || rules.Count == 0)
            {
                return new ResponseMessageNoResponse();
            }
            if (rules[0].ResponseMessage != null)
            {
                return rules[0].ResponseMessage.ToResponseMessage();
            }
            return new ResponseMessageNoResponse();
        }

    }
}