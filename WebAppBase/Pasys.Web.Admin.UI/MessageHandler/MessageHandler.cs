/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：CustomMessageHandler.cs
    文件功能描述：自定义MessageHandler
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using System.Collections.Generic;
using Pasys.Web.WeiXin;

namespace Pasys.Web.Admin.UI.MessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class MessageHandler : MessageHandler<MessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */


        //下面的Url和Token可以用其他平台的消息，或者到www.weiweihi.com注册微信用户，将自动在“微信营销工具”下得到
        //private string agentUrl =  WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        //private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
        //private string wiweihiKey = WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51

        private string appId =WorkContext.WEIXINMP_CONFIG.WeixinAppId;// WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WorkContext.WEIXINMP_CONFIG.WeixinAppSecret;// WebConfigurationManager.AppSettings["WeixinAppSecret"];

        public MessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;

            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }



        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var ruleMng = new Pasys.Web.WeiXin.RequestRuleManager();
            var rules = ruleMng.FindRules("default", RequestRuleMatchType.FullWord);
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

        //private List<Pasys.Web.WeiXin.WeiXinAutoHandlerMsgInfo> GetAutoHandlerMsgList(string key)
        //{
        //    throw new NotImplementedException();
        //}
          
    }
}
