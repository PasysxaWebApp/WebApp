using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pasys.Web.Core.EntityManager;
using System.Data.Entity;
using System.Globalization;
using Pasys.Web.Core;
namespace Pasys.Web.MemberCard
{

    public enum MemberCardStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        Init,
        /// <summary>
        /// 已开卡
        /// </summary>
        Opend,
        /// <summary>
        /// 挂失
        /// </summary>
        Losed,
        /// <summary>
        /// 作废
        /// </summary>
        Invalided
    }

    public class MemberCard : IEntity<string>
    {
        public MemberCard()
        {
            MemberCardId = Guid.NewGuid().ToString();
        }

        public string MemberCardId { get; set; }
        public string EntityName
        {
            get
            {
                return string.Format("{0}-{1}", OrganizationId, CardNo);
            }
        }
        public string OrganizationId { get; set; }
        public string UserId { get; set; }
        public string CardNo { get; set; }
        /// <summary>
        /// 现金总收入
        /// </summary>
        public long CashSum { get; set; }
        /// <summary>
        /// 会员卡目前余额
        /// </summary>
        public long Balance { get; set; }
        /// <summary>
        /// 利用状态
        /// </summary>
        public MemberCardStatus Status { get; set; }
        /// <summary>
        /// 起始有效期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束有效期
        /// </summary>
        public DateTime? EndDate { get; set; }

    }

    //public class MemberCardEntityValidator : EntityValidatorBase<MemberCard, string>
    //{
    //    private MemberCardManager _entityManager;

    //    public MemberCardEntityValidator(MemberCardManager manager)
    //        : base(manager)
    //    {
    //        _entityManager = manager;
    //    }

    //    protected override async Task ValidateEntity(MemberCard entity, List<string> errors)
    //    {
    //        if (string.IsNullOrWhiteSpace(entity.EntityName))
    //        {
    //            errors.Add(String.Format(CultureInfo.CurrentCulture, "{0}", "Name"));
    //        }
    //        else
    //        {
    //            var owner = await _entityManager.FindByCareNoAsync(entity.OrganizationId, entity.CardNo).WithCurrentCulture();
    //            if (owner != null && !EqualityComparer<string>.Default.Equals(owner.MemberCardId, entity.MemberCardId))
    //            {
    //                errors.Add(String.Format(CultureInfo.CurrentCulture, "Name {0} is already taken", entity.CardNo));
    //            }
    //        }
    //    }

    //}

    public class MemberCardStore : EntityStore<MemberCard, string>
    {
        public MemberCardStore(DbContext context) : base(context) { }
    }

    public class MemberCardManager : EntityManagerBase<MemberCard, string>
    {

        public MemberCardManager(MemberCardDbContext dbContext)
            : this(new MemberCardStore(dbContext))
        {
        }

        public MemberCardManager(MemberCardStore store)
            : base(store)
        {
            //this.EntityValidator = new MemberCardEntityValidator(this);

        }

        public  MemberCard FindByCareNo(string organizationId, string cardNo)
        {
            return AsyncHelper.RunSync(() => FindByCareNoAsync(organizationId, cardNo));
        }
        /// <summary>
        ///     Find a entity by name
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<MemberCard> FindByCareNoAsync(string organizationId, string cardNo)
        {
            if (organizationId == null)
            {
                throw new ArgumentNullException("organizationId");
            }
            if (cardNo == null)
            {
                throw new ArgumentNullException("cardNo");
            }
            return await this.Entities.Where(m => m.OrganizationId.Equals(organizationId, StringComparison.CurrentCultureIgnoreCase) && m.CardNo.Equals(cardNo, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync().WithCurrentCulture();
        }

        public void BindUser(MemberCard card, string userId) {
            AsyncHelper.RunSync(() => BindUserAsync(card, userId));
        }

        public async Task BindUserAsync(MemberCard card, string userId)
        {
            string organizationId = card.OrganizationId;
            string cardNo = card.CardNo;
            await BindUserAsync(organizationId, cardNo, userId);
            card.UserId = userId;
        }
        /// <summary>
        /// 将会员卡与用户绑定
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="cardNo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task BindUserAsync(string organizationId, string cardNo, string userId)
        {
            if (organizationId == null)
            {
                throw new ArgumentNullException("organizationId");
            }
            if (cardNo == null)
            {
                throw new ArgumentNullException("cardNo");
            }
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }
            var cards = this.Entities.Where(m => m.OrganizationId.Equals(organizationId, StringComparison.CurrentCultureIgnoreCase) && m.CardNo.Equals(cardNo, StringComparison.CurrentCultureIgnoreCase)).ToList();
            foreach (var card in cards)
            {
                card.UserId = userId;
                await this.UpdateAsync(card);
            }
        }


    }
}
