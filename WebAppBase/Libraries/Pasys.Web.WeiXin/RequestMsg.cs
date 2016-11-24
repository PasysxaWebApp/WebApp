using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class RequestMsg
    {
        #region Model
        private long _usermsgid;
        private string _openid;
        private string _username;
        private string _msgtype;
        private DateTime _createtime;
        private string _description;
        private string _location_x;
        private string _location_y;
        private int _scale;
        private string _picurl;
        private string _title;
        private string _url;
        private string _event;
        private string _eventkey;
        /// <summary>
        /// 
        /// </summary>
        public long UserMsgId
        {
            set { _usermsgid = value; }
            get { return _usermsgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OpenId
        {
            set { _openid = value; }
            get { return _openid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
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
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Location_X
        {
            set { _location_x = value; }
            get { return _location_x; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Location_Y
        {
            set { _location_y = value; }
            get { return _location_y; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Scale
        {
            set { _scale = value; }
            get { return _scale; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PicUrl
        {
            set { _picurl = value; }
            get { return _picurl; }
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
        public string Url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Event
        {
            set { _event = value; }
            get { return _event; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventKey
        {
            set { _eventkey = value; }
            get { return _eventkey; }
        }
        #endregion Model}
    }
}