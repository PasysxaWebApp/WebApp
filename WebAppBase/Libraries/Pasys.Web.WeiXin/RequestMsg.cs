using Pasys.Core.EntityManager;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pasys.Web.WeiXin
{

    public class ApplicationRequestMessageBase : IEntity<string>
    {
        public long MsgId { get; set; }
        public virtual int MsgType { get; set; }
        public RequestMsgType RequestMsgType
        {
            get
            {
                return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), MsgType.ToString());
            }
        }
        public string Encrypt { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}
        /// <summary>
        /// 消息已处理
        /// </summary>
        public bool IsProcessed { get; set; }
        /// <summary>
        /// 处理用户
        /// </summary>
        public string ProcUserId { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcDateTime { get; set; }
        public string EntityName
        {
            get {
                return string.Format("{0}", MsgId);
            }
        }
    }
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ApplicationRequestMessageImage : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Image); }
            set { }
        }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }
    }
    /// <summary>
    /// 链接消息
    /// </summary>
    public class ApplicationRequestMessageLink : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Link); }
            set { }
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
    }
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class ApplicationRequestMessageLocation : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Location); }
            set { }
        }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
    }
    /// <summary>
    /// 小视频消息
    /// </summary>
    public class ApplicationRequestMessageShortVideo : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.ShortVideo); }
            set { }
        }

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ApplicationRequestMessageText : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Text); }
            set { }
        }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

    }
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ApplicationRequestMessageVideo : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Video); }
            set { }
        }

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }
    }
    /// <summary>
    /// 语音消息
    /// </summary>
    public class ApplicationRequestMessageVoice : ApplicationRequestMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(RequestMsgType.Voice); }
            set { }
        }

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音格式：amr
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// 开通语音识别功能，用户每次发送语音给公众号时，微信会在推送的语音消息XML数据包中，增加一个Recongnition字段。
        /// 注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效。开发者可以重新关注此帐号进行测试。
        /// </summary>
        public string Recognition { get; set; }
    }


    public class RequestMessageStore : EntityStore<ApplicationRequestMessageBase, string>
    {
        public RequestMessageStore(DbContext context) : base(context) { }
    }

    public class RequestMessageManager : EntityManagerBase<ApplicationRequestMessageBase, string>
    {
        public RequestMessageManager()
            : this(WeiXinDbContext.Create())
        { }

        public RequestMessageManager(WeiXinDbContext dbContext)
            : this(new RequestMessageStore(dbContext))
        {
        }

        public RequestMessageManager(RequestMessageStore store)
            : base(store)
        {
            //this.EntityValidator = new MPEntityValidator(this);
        }


        public void AddImageMessage(ApplicationRequestMessageImage request)
        {
            this.Create(request);
        }
        public void AddMusicMessage(ApplicationRequestMessageLink request)
        {
            this.Create(request);
        }
        public void AddNewsMessage(ApplicationRequestMessageLocation request)
        {
            this.Create(request);
        }
        public void AddTextMessage(ApplicationRequestMessageShortVideo request)
        {
            this.Create(request);
        }
        public void AddTransferCustomerServiceMessage(ApplicationRequestMessageText request)
        {
            this.Create(request);
        }
        public void AddVideoMessage(ApplicationRequestMessageVideo request)
        {
            this.Create(request);
        }
        public void AddVoiceMessage(ApplicationRequestMessageVoice request)
        {
            this.Create(request);
        }

    }


}