﻿/*----------------------------------------------------------------
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
using Pasys.Web.Core;

namespace Pasys.Web.WeiXin.UI.MessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
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

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
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

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (string.IsNullOrWhiteSpace(requestMessage.Content))
            {
                responseMessage.Content = string.Format("输入menu试试！");
            }
            else if (requestMessage.Content == "幸运")
            {
                responseMessage.Content = string.Format("您是幸运之星，找到了彩蛋！");
            }
            else
            {
                var list = GetAutoHandlerMsgList(requestMessage.Content);
                if (list == null || list.Count == 0)
                {
                    list = GetAutoHandlerMsgList("menu");
                }
                var result = new StringBuilder();
                foreach (var msg in list)
                {
                    result.AppendFormat("{0}", msg.MsgContent);
                    result.AppendLine();
                }
                responseMessage.Content = result.ToString();
            }
            return responseMessage;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        //{
        //    var locationService = new LocationService();
        //    var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
        //    return responseMessage;
        //}

        //public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        //{
        //    var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您刚才发送的是小视频";
        //    return responseMessage;
        //}

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageNews>();
        //    responseMessage.Articles.Add(new Article()
        //    {
        //        Title = "您刚才发送了图片信息",
        //        Description = "您发送的图片将会显示在边上",
        //        PicUrl = requestMessage.PicUrl,
        //        Url = "http://weixin.senparc.com"
        //    });
        //    responseMessage.Articles.Add(new Article()
        //    {
        //        Title = "第二条",
        //        Description = "第二条带连接的内容",
        //        PicUrl = requestMessage.PicUrl,
        //        Url = "http://weixin.senparc.com"
        //    });

        //    return responseMessage;
        //}

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
        //    //上传缩略图
        //    var accessToken = CommonAPIs.AccessTokenContainer.TryGetToken(appId, appSecret);
        //    var uploadResult = AdvancedAPIs.MediaApi.UploadTemporaryMedia(accessToken, UploadMediaFileType.image,
        //                                                 Server.GetMapPath("~/Images/Logo.jpg"));

        //    //设置音乐信息
        //    responseMessage.Music.Title = "天籁之音";
        //    responseMessage.Music.Description = "播放您上传的语音";
        //    responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
        //    responseMessage.Music.HQMusicUrl = "http://weixin.senparc.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
        //    responseMessage.Music.ThumbMediaId = uploadResult.media_id;
        //    return responseMessage;
        //}
        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
        //    return responseMessage;
        //}

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        //        {
        //            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //            responseMessage.Content = string.Format(@"您发送了一条连接信息：
        //Title：{0}
        //Description:{1}
        //Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
        //            return responseMessage;
        //        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var subscribeEvent = requestMessage as RequestMessageEvent_Subscribe;
            if (subscribeEvent != null)
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                var list = GetAutoHandlerMsgList("subscribe");
                var result = new StringBuilder();
                foreach (var msg in list)
                {
                    result.AppendFormat("{0}", msg.MsgContent);
                    result.AppendLine();
                }
                responseMessage.Content = result.ToString();
                return responseMessage;
            }

            var clickEvent = requestMessage as RequestMessageEvent_Click;
            if (clickEvent != null)
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                var list = GetAutoHandlerMsgList(clickEvent.EventKey);
                if (list == null || list.Count == 0)
                {
                    list = GetAutoHandlerMsgList("menu");
                }
                var result = new StringBuilder();
                foreach (var msg in list)
                {
                    result.AppendFormat("{0}", msg.MsgContent);
                    result.AppendLine();
                }
                responseMessage.Content = result.ToString();
                return responseMessage;
            }


            //var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            ////TODO: 对Event信息进行统一操作
            //return eventResponseMessage;
            var responseMessageOther = this.CreateResponseMessage<ResponseMessageText>();
            responseMessageOther.Content = "";// "未识别的消息。";
            return responseMessageOther;

        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";// "未识别的消息。";
            return responseMessage;
        }

        private List<WeiXinAutoHandlerMsgInfo> GetAutoHandlerMsgList(string key)
        {
            throw new NotImplementedException();
        }
          
    }
}
