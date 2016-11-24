using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class PostMsgItem
    {
        #region Model
        private int _postmsgid;
        private int _itemid;
        private int _type = 0;
        /// <summary>
        /// 
        /// </summary>
        public int PostMsgId
        {
            set { _postmsgid = value; }
            get { return _postmsgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ItemId
        {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        #endregion Model
    }
}
