using Pasys.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class UserBind : IEntity<string>
    {
        /// <summary>
        /// Key
        /// </summary>
        public string UserBinderId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }


        //public UserInfo WxUser { get; set; }

        /// <summary>
        /// IEntity
        /// </summary>
        public string EntityName
        {
            get
            {
                return string.Format("{0}-{1}", UserId, OpenId);
            }
        }
    }

    public class UserBindStore : EntityStore<UserBind, string>
    {
        public UserBindStore(DbContext context) : base(context) { }
    }

    public class UserBindManager : EntityManagerBase<UserBind, string>
    {
        public UserBindStore BizStore
        {
            get
            {
                return (UserBindStore)this.Store;
            }
        }


        public UserBindManager()
            : this(WeiXinDbContext.Create())
        { }

        public UserBindManager(WeiXinDbContext dbContext)
            : this(new UserBindStore(dbContext))
        {
        }

        public UserBindManager(UserBindStore store)
            : base(store)
        {
            //this.EntityValidator = new MPEntityValidator(this);

        }

        public bool BindUser(string UserId, string OpenId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                throw new ArgumentNullException("UserId");
            }
            if (string.IsNullOrEmpty(OpenId))
            {
                throw new ArgumentNullException("OpenId");
            }

            var bu = this.BizStore.Entities.FirstOrDefault(m => m.OpenId.Equals(OpenId, StringComparison.CurrentCultureIgnoreCase));
            if (bu == null)
            {
                bu = new UserBind
                {
                    UserBinderId = Guid.NewGuid().ToString(),
                    OpenId = OpenId,
                    UserId = UserId
                };
                var vr = this.Create(bu);
                return vr.Succeeded;
            }
            else
            {
                bu.UserId = UserId;
                var vr = this.Update(bu);
                return vr.Succeeded;
            }
        }

        public string GetUserId(string OpenId)
        {
            var bu = this.BizStore.Entities.FirstOrDefault(m => m.OpenId.Equals(OpenId, StringComparison.CurrentCultureIgnoreCase));
            if (bu != null)
            {
                return bu.UserId;
            }
            return string.Empty;
        }

        public string GeOpenId(string UserId)
        {
            var bu = this.BizStore.Entities.FirstOrDefault(m => m.UserId.Equals(UserId, StringComparison.CurrentCultureIgnoreCase));
            if (bu != null)
            {
                return bu.OpenId;
            }
            return string.Empty;
        }
    }



}
