using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class PostMsg
    {
        #region Model
        private long _postmsgid;
        private int _ruleid = -1;
        private string _msgtype;
        private DateTime _createtime;
        private string _title;
        private string _description;
        private string _musicurl;
        private string _hqmusicurl;
        private int _articlecount;
        /// <summary>
        /// 
        /// </summary>
        public long PostMsgId
        {
            set { _postmsgid = value; }
            get { return _postmsgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RuleId
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgType
        {
            set { _msgtype = value; }
            get { return _msgtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MusicUrl
        {
            set { _musicurl = value; }
            get { return _musicurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HQMusicUrl
        {
            set { _hqmusicurl = value; }
            get { return _hqmusicurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ArticleCount
        {
            set { _articlecount = value; }
            get { return _articlecount; }
        }
        #endregion Model
    }
}
