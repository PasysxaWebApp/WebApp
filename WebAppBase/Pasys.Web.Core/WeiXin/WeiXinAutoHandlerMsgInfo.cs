using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core
{
    public class WeiXinAutoHandlerMsgInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int MsgID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MsgKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MsgContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ActionID { get; set; }
    }
}
