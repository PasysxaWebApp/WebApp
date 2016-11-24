using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class KeyRuleValue
    {
        #region Model
        private int _valueid;
        private int _ruleid = -1;
        private string _value;
        private int _matchtype = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ValueId
        {
            set { _valueid = value; }
            get { return _valueid; }
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
        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MatchType
        {
            set { _matchtype = value; }
            get { return _matchtype; }
        }
        #endregion Model
    }
}
