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

        public virtual ResponseMessageBase ToResponseMessage()
        {
            throw new NotImplementedException();
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

        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageImage();
            res.Image.MediaId = this.Image.MediaId;
            return res;
        }

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
        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageMusic();
            res.Music.Title = this.Music.Title;
            res.Music.Description = this.Music.Description;
            res.Music.MusicUrl = this.Music.MusicUrl;
            res.Music.HQMusicUrl = this.Music.HQMusicUrl;
            res.Music.ThumbMediaId = this.Music.ThumbMediaId;
            return res;
        }

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

        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageNews();
            foreach (var art in this.Articles)
            {
                var a = new Article()
                {
                    Title = art.Title,
                    Description = art.Description,
                    PicUrl = art.PicUrl,
                    Url = art.Url,
                };
                res.Articles.Add(a);
            }
            return res;
        }
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
        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageText();
            res.Content = this.Content;
            return res;
        }

    }

    public class ApplicationResponseMessageTransferCustomerService : ApplicationResponseMessageBase
    {
        public override int MsgType
        {
            get { return Convert.ToInt32(ResponseMsgType.Transfer_Customer_Service); }
            set { }
        }

        public List<ApplicationCustomerServiceAccount> TransInfo { get; set; }
        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageTransfer_Customer_Service();
            foreach (var tras in this.TransInfo)
            {
                var a = new CustomerServiceAccount()
                {
                    KfAccount = tras.KfAccount,
                };
                res.TransInfo.Add(a);
            }
            return res;
        }

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
        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageVideo();
            res.Video.MediaId = this.Video.MediaId;
            res.Video.Title = this.Video.Title;
            res.Video.Description = this.Video.Description;
            return res;
        }

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
        public virtual ResponseMessageBase ToResponseMessage()
        {
            var res = new ResponseMessageVoice();
            res.Voice.MediaId = this.Voice.MediaId;
            return res;
        }

    }

    public class ApplicationVoice
    {
        public string MediaId { get; set; }
    }



    public class ResponseMessageStore : EntityStore<ApplicationResponseMessageBase, string>
    {
        public ResponseMessageStore(DbContext context) : base(context) { }

    }

    public class ResponseMessageManager : EntityManagerBase<ApplicationResponseMessageBase, string>
    {
        public ResponseMessageStore BizStore
        {
            get
            {
                return (ResponseMessageStore)this.Store;
            }
        }

        public ResponseMessageManager()
            : this(WeiXinDbContext.Create())
        { }

        public ResponseMessageManager(WeiXinDbContext dbContext)
            : this(new ResponseMessageStore(dbContext))
        {
        }

        public ResponseMessageManager(ResponseMessageStore store)
            : base(store)
        {
            //this.EntityValidator = new MPEntityValidator(this);
        }

        public void AddImageMessage(ApplicationResponseMessageImage response)
        {
            this.Create(response);
        }
        public void AddMusicMessage(ApplicationResponseMessageMusic response)
        {
            this.Create(response);
        }
        public void AddNewsMessage(ApplicationResponseMessageNews response)
        {
            this.Create(response);
        }
        public void AddTextMessage(ApplicationResponseMessageText response)
        {
            this.Create(response);
        }
        public void AddTransferCustomerServiceMessage(ApplicationResponseMessageTransferCustomerService response)
        {
            this.Create(response);
        }
        public void AddVideoMessage(ApplicationResponseMessageVideo response)
        {
            this.Create(response);
        }
        public void AddVoiceMessage(ApplicationResponseMessageVoice response)
        {
            this.Create(response);
        }
    }


}
