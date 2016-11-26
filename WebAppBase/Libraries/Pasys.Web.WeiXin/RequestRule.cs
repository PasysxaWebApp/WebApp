using Pasys.Web.Core.EntityManager;
using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public enum RequestRuleMatchType
    {
        /// <summary>
        /// 全字匹配
        /// </summary>
        FullWord,
        /// <summary>
        /// 模糊匹配
        /// </summary>
        Fuzzy
    }
    public class RequestRule : IEntity<string>
    {
        /// <summary>
        /// Key
        /// </summary>
        public long RuleId { get; set; }
        /// <summary>
        /// 规则名
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>        
        public RequestMsgType RequestMsgType { get; set; }
        /// <summary>
        /// 匹配关键字
        /// </summary>
        public string MatchKey { get; set; }
        /// <summary>
        /// 匹配类型
        /// </summary>
        public RequestRuleMatchType MatchType { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ResponseId { get; set; }

        public virtual ApplicationResponseMessageBase ResponseMessage { get; set; }
        /// <summary>
        /// 自定义的处理程序名字
        /// </summary>
        public string ModelFunctionName { get; set; }
        public string EntityName
        {
            get
            {
                return RuleName;
            }
            set
            {
                RuleName = value;
            }
        }
        
    }

    public class RequestRuleStore : EntityStore<RequestRule, string>
    {
        public RequestRuleStore(DbContext context) : base(context) { }
    }

    public class RequestRuleManager : EntityManagerBase<RequestRule, string>
    {
        public RequestRuleManager()
            : this(WeiXinDbContext.Create())
        { }

        public RequestRuleManager(WeiXinDbContext dbContext)
            : this(new RequestRuleStore(dbContext))
        {
        }

        public RequestRuleManager(RequestRuleStore store)
            : base(store)
        {
            //this.EntityValidator = new MPEntityValidator(this);
        }

        public  List<RequestRule> FindRules(string matchKey)
        {
            var rules = this.Entities.Where(m => (m.MatchKey.Equals(matchKey) && m.MatchType == RequestRuleMatchType.FullWord) || (m.MatchKey.Contains(matchKey) && m.MatchType == RequestRuleMatchType.Fuzzy));
            return rules.ToList();
        }

        public List<RequestRule> FindRules(string matchKey, RequestRuleMatchType matchType)
        {
            IQueryable<RequestRule> rules;
            if (matchType == RequestRuleMatchType.FullWord)
            {
                rules = this.Entities.Where(m => m.MatchKey.Equals(matchKey) && m.MatchType == RequestRuleMatchType.FullWord);
            }
            else
            {
                rules = this.Entities.Where(m => m.MatchKey.Contains(matchKey) && m.MatchType == RequestRuleMatchType.Fuzzy);
            }
            return rules.ToList();
        }


    }


}
