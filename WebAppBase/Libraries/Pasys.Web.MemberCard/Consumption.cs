using Pasys.Web.Core.EntityManager;
using SharedUtilitys.DataBases;
using SharedUtilitys.DataBases.Converters;
using SharedUtilitys.DataBases.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.MemberCard
{
    public enum ConsueType
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        Init,
        /// <summary>
        /// 开卡
        /// </summary>
        Open,
        /// <summary>
        /// 消费
        /// </summary>
        Consume,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge,
        /// <summary>
        /// 挂失
        /// </summary>
        Loss,
        /// <summary>
        /// 解除挂失
        /// </summary>
        LiftLoss,
        /// <summary>
        /// 作废
        /// </summary>
        Invalid
    }

    public class Consumption : IEntity<long>
    {
        public Consumption()
        {
            Cash = 0;
            Amount = 0;
            ConsueDateTime = DateTime.Now;
            CreateDateTime = DateTime.Now;
        }
        public long ConsumptionId { get; set; }
        public string EntityName
        {
            get
            {
                return string.Format("{0}", ConsumptionId);
            }
        }

        // Foreign key
        public string MemberCardId { get; set; }

        /// <summary>
        /// 消费类型
        /// </summary>
        public int ConsueType { get; set; }
        /// <summary>
        /// 消费日期时间
        /// </summary>
        public DateTime ConsueDateTime { get; set; }
        /// <summary>
        /// 现金总收入
        /// </summary>
        public long Cash { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// 消费商品
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 操作日期时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        // Navigation properties
        public virtual MemberCard MemberCard { get; set; }

    }

    //public class ConsumptionEntityValidator : EntityValidatorBase<Consumption, long>
    //{
    //    public ConsumptionEntityValidator(ConsumptionManager manager)
    //        : base(manager)
    //    {
    //    }
    //}

    public class ConsumptionStore : EntityStore<Consumption, long>
    {
        public ConsumptionStore(DbContext context) : base(context) { }
    }

    public class ConsumptionManager : EntityManagerBase<Consumption, long>
    {
        public ConsumptionStore BizStore
        {
            get
            {
                return (ConsumptionStore)this.Store;
            }
        }

        public ConsumptionManager()
            : this(MemberCardDbContext.Create())
        { }

        public ConsumptionManager(MemberCardDbContext dbContext)
            : this(new ConsumptionStore(dbContext))
        {
        }

        public ConsumptionManager(ConsumptionStore store)
            : base(store)
        {
            //this.EntityValidator = new ConsumptionEntityValidator(this);
        }
        /// <summary>
        /// 开卡
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool OpenCard(string memberCardId, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.Open, 0, 0, "开卡", createUserId, note);
        }
        /// <summary>
        /// 挂失卡
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool LossCard(string memberCardId, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.Loss, 0, 0, "挂失", createUserId, note);
        }
        /// <summary>
        /// 解除挂失
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool LiftLossCard(string memberCardId, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.LiftLoss, 0, 0, "解挂", createUserId, note);
        }
        /// <summary>
        /// 作废卡
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool InvalidCard(string memberCardId, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.Invalid, 0, 0, "作废", createUserId, note);
        }
        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="amount"></param>
        /// <param name="productName"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool Consume(string memberCardId, long amount, string productName, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.Consume, 0, amount, productName, createUserId, note);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="memberCardId"></param>
        /// <param name="cash"></param>
        /// <param name="amount"></param>
        /// <param name="createUserId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool Recharge(string memberCardId, long cash, long amount, string createUserId, string note)
        {
            return AddRecorde(memberCardId, ConsueType.Recharge, cash, amount, "充值", createUserId, note);
        }

        public bool AddRecorde(string memberCardId, ConsueType consueType, long cash, long amount, string productName, string createUserId, string note)
        {
            var consu = new Consumption()
            {
                MemberCardId = memberCardId,
                ConsueType = Convert.ToInt32(consueType),
                Cash = 0,
                Amount = 0,
                ProductName = productName,
                CreateUserId = createUserId,
                Note = note
            };
            if (consueType == ConsueType.Consume)
            {
                consu.Amount = amount;
            }
            else if (consueType == ConsueType.Recharge)
            {
                consu.Amount = amount * -1;
                consu.Cash = cash;
            }
            var sql = new StringBuilder();
            var dbcontex = BizStore.Context;
            var trans = dbcontex.Database.BeginTransaction();
            try
            {
                dbcontex.Set<Consumption>().Add(consu);
                dbcontex.SaveChanges();
                if (consu.Amount != 0 || consu.Cash != 0)
                {
                    sql.Append(@"Update mc_m_membercards set Balance=Balance-@Amount,CashSum=CashSum+@Cash where MemberCardId=@MemberCardId");
                    var p_amount = DbParamConverter.Convert(new SqlParamWrapper("Amount", Convert.ToInt32(consu.Amount)));
                    var p_cash = DbParamConverter.Convert(new SqlParamWrapper("Cash", Convert.ToInt32(consu.Cash)));
                    var p_memberCardId = DbParamConverter.Convert(new SqlParamWrapper("MemberCardId", memberCardId));
                    dbcontex.Database.ExecuteSqlCommand(sql.ToString(), new object[] { p_amount, p_cash, p_memberCardId });
                }
                else
                {
                    var cardStatus = MemberCardStatus.Init;
                    if (consueType == ConsueType.Open)
                    {
                        cardStatus = MemberCardStatus.Opend;
                    }
                    else if (consueType == ConsueType.Loss)
                    {
                        cardStatus = MemberCardStatus.Losed;
                    }
                    else if (consueType == ConsueType.LiftLoss)
                    {
                        cardStatus = MemberCardStatus.Opend;
                    }
                    else if (consueType == ConsueType.Invalid)
                    {
                        cardStatus = MemberCardStatus.Invalided;
                    }
                    if (cardStatus != MemberCardStatus.Init)
                    {
                        sql.Append(@"Update mc_m_membercards set Status=@Status where MemberCardId=@MemberCardId");

                        var p_status = DbParamConverter.Convert(new SqlParamWrapper("Status", Convert.ToInt32(cardStatus)));
                        var p_memberCardId = DbParamConverter.Convert(new SqlParamWrapper("MemberCardId", memberCardId));
                        dbcontex.Database.ExecuteSqlCommand(sql.ToString(), p_status, p_memberCardId);
                    }
                }
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback();
                return false;
            }
        }

    }
}
