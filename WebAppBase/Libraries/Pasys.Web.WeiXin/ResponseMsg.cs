using Pasys.Web.Core.EntityManager;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class ApplicationResponseMessageBase : IEntity<string>
    {
        /// <summary>
        /// Key
        /// </summary>
        public string ResponseId { get; set; }
        public virtual int MsgType { get; set; }
        public ResponseMsgType ResponseMsgType
        {
            get
            {
                return (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), MsgType.ToString());
            }
        }
        public string EntityName
        {
            get
            {
                return string.Format("{0}", ResponseId);
            }
        }
    }

    public class ApplicationResponseMessageImage : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Image); }
            set { }
        }

        public ApplicationImage Image { get; set; }
    }

    public class ApplicationImage
    {
        public string MediaId { get; set; }

    }

    public class ApplicationResponseMessageMusic : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Image); }
            set { }
        }

        public ApplicationMusic Music { get; set; }

    }

    public class ApplicationMusic
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string MusicUrl { get; set; }
        public string HQMusicUrl { get; set; }
        ///// <summary>
        ///// 缩略图的媒体id，通过上传多媒体文件，得到的id
        ///// 官方API上有，但是加入的话会出错
        ///// </summary>
        public string ThumbMediaId { get; set; }

    }

    public class ApplicationResponseMessageNews : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Image); }
            set { }
        }

        public int ArticleCount
        {
            get
            {
                return Articles == null ? 0 : Articles.Count;
            }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        public List<ApplicationArticle> Articles { get; set; }
    }

    public class ApplicationArticle
    {
        public string ArticleId { get; set; }
        public string ResponseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
        public virtual ApplicationResponseMessageNews ResponseMessage { get; set; }
    }

    public class ApplicationResponseMessageText : ApplicationResponseMessageBase
    {
         public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Text); }
            set { }
        }

        public string Content { get; set; }

    }

    public class ApplicationResponseMessageTransferCustomerService : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Transfer_Customer_Service); }
            set { }
        }

        public List<ApplicationCustomerServiceAccount> TransInfo { get; set; }

    }

    public class ApplicationCustomerServiceAccount
    {
        public string CustomerServiceAccountId { get; set; }
        public string ResponseId { get; set; }
        public string KfAccount { get; set; }
        public virtual ApplicationResponseMessageTransferCustomerService ResponseMessage { get; set; }
    }

    public class ApplicationResponseMessageVideo : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Video); }
            set { }
        }

        public ApplicationVideo Video { get; set; }

    }

    public class ApplicationVideo
    {
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ApplicationResponseMessageVoice : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Voice); }
            set { }
        }

        public ApplicationVoice Voice { get; set; }
    }

    public class ApplicationVoice
    {
        public string MediaId { get; set; }
    }

}
