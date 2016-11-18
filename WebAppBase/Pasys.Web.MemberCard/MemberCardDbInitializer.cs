using System.Data.Entity;
using Pasys.Web.Core.EntityManager;

namespace Pasys.Web.MemberCard
{
    public class MemberCardDbInitializer : DropCreateDatabaseIfModelChanges<MemberCardDbContext>
    {
        protected override void Seed(MemberCardDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //创建用户名为admin@123.com，密码为“Admin@123456”并把该用户添加到角色组"Admin"中
        public static void InitializeIdentityForEF(MemberCardDbContext db)
        {
            /*             
                drop table [dbo].[__MigrationHistory];
                drop table [dbo].[mc_m_membercards];
                drop table [dbo].[mc_t_consumptions];
             */

            var userId = "915ad524-3d85-4a09-9c3f-2a1468280209";

            var mc = new MemberCard()
            {
                OrganizationId = "DebugOrganizationID",
                CardNo = "0000001",
                Status = MemberCardStatus.Init
            };
            var cardMng = new MemberCardManager(db);
            var extCard = cardMng.FindByCareNo(mc.OrganizationId,mc.CardNo);
            if (extCard==null)
            {
                cardMng.Create(mc);
                cardMng.BindUser(mc, userId);
                var consumeMng = new ConsumptionManager(db);
                consumeMng.OpenCard(mc.MemberCardId, userId, "开卡啦");
                consumeMng.Recharge(mc.MemberCardId, 1000000, 10000000, userId, "我是土豪");
                consumeMng.Consume(mc.MemberCardId, 999999,"海天盛宴", userId, "我会败家");
                consumeMng.InvalidCard(mc.MemberCardId, userId, "败家败完了");
            }

        }
    }
}
