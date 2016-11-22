using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class WeiXinDbContext : DbContext
    {
        public WeiXinDbContext()
            : base("DefaultConnection")
        {
        }

        DbSet<MP> MPs { get; set; }
        DbSet<MPMenuInfo> MPMenuInfos { get; set; }
        DbSet<UserBind> UserBinders { get; set; }
        DbSet<WeiXinUserInfo> UserInfos { get; set; }


        public static void CreateForce()
        {
            var db = new WeiXinDbContext();
            db.Database.Initialize(false);
            WeiXinDbInitializer.InitializeIdentityForEF(db);
        }


        public static WeiXinDbContext Create()
        {
            // 在第一次启动网站时初始化数据库            
            //Database.SetInitializer<MemberCardDbContext>(new MemberCardDbInitializer());
            return new WeiXinDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var membercard = modelBuilder.Entity<MP>()
            .HasKey(m => m.MPId)
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_mps");
            //membercard.Property(m => m.MemberCardId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


            var consumption = modelBuilder.Entity<MPMenuInfo>()
            .HasKey(m => m.MenuId)
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_mpmenuinfos");
            //consumption.Property(m => m.ConsumptionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            var userinfo = modelBuilder.Entity<WeiXinUserInfo>()
            .HasKey(m => m.OpenId)
            .Ignore(m => m.EntityName)            
            .ToTable("weixin_m_userinfos");

            var userbind = modelBuilder.Entity<UserBind>()
            .HasKey(m => m.UserBinderId)            
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_userbinds");
            //1：1，并且级联删除
            //userbind.HasRequired(m => m.WxUser).WithOptional().WillCascadeOnDelete();

        }
    }

}
