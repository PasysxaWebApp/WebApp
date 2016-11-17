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
        
        public string EntityName
        {
            get
            {
                return "";
            }
        }
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
        public ConsumptionManager(ConsumptionStore store)
            : base(store)
        {
            this.EntityValidator = new ConsumptionEntityValidator(this);
        }

        public bool AddConsume(string MemberCardId,ConsueType consueType, long amount, string ProductName, string CreateUserId, string Note)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    Insert into mc_t_consumptions 
                        `t_notice`.`NoticeID`,
                        `t_notice`.`OrganizationID`,
                        `t_notice`.`Title`,
                        `t_notice`.`ContentTxt`,
                        `t_notice`.`CreateUserID`,
                        `t_notice`.`CreateDateTime`,
                        `t_notice`.`LastUserID`,
                        `t_notice`.`LastUpdatetime`,
                        `t_notice`.`Sticky`,
                        createUser.UserName as CreateUserName,
                        updateUser.UserName as UpdateUserName
                    FROM `t_notice` 
                    left join m_user createUser on createUser.Guid=`t_notice`.`CreateUserID`
                    left join m_user updateUser on updateUser.Guid=`t_notice`.`LastUserID`
                    where ");
               

                utility.BeginTransaction();
                try
                {
                    utility.AddParameter("MemberCardId", MemberCardId);
                    utility.ExecuteNonQuery(sql.ToString());
                    utility.Commit();
                    return true;
                }
                catch (Exception)
                {
                    utility.Rollback();
                    return false;
                }
                
            }
        }

    }
}
