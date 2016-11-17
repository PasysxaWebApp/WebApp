using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.MemberCard
{
    public class MemberCardDbContext : DbContext
    {
        public MemberCardDbContext()
            : base("DefaultConnection")
        {
        }

        DbSet<Consumption> Consumptions { get; set; }
        DbSet<MemberCard> MemberCards { get; set; }

        public static MemberCardDbContext Create()
        {
            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库            
            return new MemberCardDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var membercard = modelBuilder.Entity<MemberCard>()
            .HasKey(m => m.MemberCardId)
            .Ignore(m=>m.EntityName)
            .ToTable("mc_m_membercards");
            membercard.Property(m => m.MemberCardId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            var consumption = modelBuilder.Entity<Consumption>()
            .HasKey(m => m.ConsumptionId)
            .ToTable("mc_t_consumptions");
            consumption.Property(m => m.MemberCardId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            consumption.HasRequired(m => m.MemberCard).WithMany().HasForeignKey(m => m.MemberCardId);


        }
    }
}
