using Pasys.Web.Core.EntityManager;
using SharedUtilitys.DataBases;
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
        Charge,
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

    public class Consumption:IEntity<long>
    {
        public long ConsumptionId { get; set; }
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

    public class ConsumptionEntityValidator : EntityValidatorBase<Consumption, long>
    {
        public ConsumptionEntityValidator(ConsumptionManager manager)
            : base(manager)
        {
        }
    }

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
        public ConsumptionManager(ConsumptionStore store)
            : base(store)
        {
            this.EntityValidator = new ConsumptionEntityValidator(this);
        }

        public bool AddConsume(string memberCardId,ConsueType consueType, long amount, string productName, string createUserId, string note)
        {            
            var consu=  new Consumption(){
                MemberCardId=memberCardId,
                ConsueType = Convert.ToInt32(consueType),
                CreateUserId=createUserId,
                CreateDateTime=DateTime.Now,
                Note=note
            };
            if (consueType == ConsueType.Consume )
            {
                consu.Amount = amount;
            }
            else if (consueType == ConsueType.Charge)
            {
                consu.Amount = amount * -1;
            }
            var sql= new StringBuilder();
            sql.Append(@"Update mc_m_membercards set Balance=Balance-@Amount where MemberCardId=@MemberCardId");

            var dbcontex = BizStore.Context;
            var trans= dbcontex.Database.BeginTransaction();
            try
            {
                dbcontex.Set<Consumption>().Add(consu);
                dbcontex.Database.ExecuteSqlCommand(sql.ToString(), new object[] { consu.Amount, memberCardId });
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
