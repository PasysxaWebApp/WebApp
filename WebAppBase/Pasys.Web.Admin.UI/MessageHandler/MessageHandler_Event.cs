using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Pasys.Core.EntityManager;
using Pasys.Web.WeiXin;

namespace Pasys.Web.Admin.UI.MessageHandler
{
    public partial class MessageHandler : MessageHandler<MessageContext>
    {

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            //requestMessage.Ticket
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var ruleMng = new Pasys.Web.WeiXin.RequestRuleManager();
            var rules = ruleMng.FindRules("subscribe", RequestRuleMatchType.FullWord);
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

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //requestMessage.Ticket;
            return base.OnEvent_ScanRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var ruleMng = new Pasys.Web.WeiXin.RequestRuleManager();
            var rules = ruleMng.FindRules(requestMessage.EventKey, RequestRuleMatchType.FullWord);
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