using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public enum Authorization
    {
        Deny,
        Allow,
    }
    
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //static ApplicationDbContext()
        //{         
        //}

        DbSet<ApplicationMenu> Menus { get; set; }
        DbSet<ApplicationRoleMenu> RoleMenus { get; set; }
        DbSet<ApplicationOrganization> Organizations { get; set; }

        public static AppIdentityDbContext Create(IdentityFactoryOptions<AppIdentityDbContext> options, IOwinContext context)
        {
            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库
            var db = new AppIdentityDbContext();
            Database.SetInitializer<AppIdentityDbContext>(new AppIdentityDbInitializer());
            return db;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var organiation = modelBuilder.Entity<ApplicationOrganization>()
                .HasKey(m => m.OrganizationId)
                .ToTable("account_m_organizations");
            organiation.Property(m => m.OrganizationId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<IdentityRole>().ToTable("account_m_roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("account_m_userroles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("account_m_userlogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("account_m_userclaims");

            var role=modelBuilder.Entity<ApplicationRole>().ToTable("account_m_roles");
            role.HasRequired(m => m.Organization).WithMany(c => c.Roles).HasForeignKey(c => c.OrganizationId);

            var user= modelBuilder.Entity<ApplicationUser>()
                .HasKey(p => p.Id)
                .ToTable("account_m_users");

            var menu = modelBuilder.Entity<ApplicationMenu>()
                .HasKey(m => m.MenuId)
                .Ignore(m => m.IsRootMenu)
                .Ignore(m => m.ShowInMenu)
                .Ignore(m => m.SeparateMenuFlag)
                .ToTable("account_m_menus");
            menu.Property(m => m.MenuId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ApplicationRoleMenu>().HasRequired(m => m.Role).WithMany(c => c.RoleMenus).HasForeignKey(c => c.RoleId);
            //modelBuilder.Entity<ApplicationRoleMenu>().HasRequired(m => m.Menu).WithMany().HasForeignKey(c => c.MenuId);

            //modelBuilder.Entity<ApplicationMenu>().HasRequired(m => m.ParentMenu).WithMany(c => c.SubMenus).HasForeignKey(c => c.MenuId);

            var roleMenu = modelBuilder.Entity<ApplicationRoleMenu>()
                .HasKey(m => new { m.RoleId, m.MenuId })
                .ToTable("account_m_rolemenus");


            //roleMenu.Property(m => m.RoleMenuId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //// Keep this:
            //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            //// Change TUser to ApplicationUser everywhere else - 
            //// IdentityUser and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
            //EntityTypeConfiguration<ApplicationUser> table =
            //    modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            //table.Property((ApplicationUser u) => u.UserName).IsRequired();

            //// EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            //modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            //modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
            //    new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            //// Leave this alone:
            //EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
            //    modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
            //        new
            //        {
            //            UserId = l.UserId,
            //            LoginProvider = l.LoginProvider,
            //            ProviderKey
            //                = l.ProviderKey
            //        }).ToTable("AspNetUserLogins");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            //EntityTypeConfiguration<IdentityUserClaim> table1 =
            //    modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            //// Add this, so that IdentityRole can share a table with ApplicationRole:
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            //// Change these from IdentityRole to ApplicationRole:
            //EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
            //    modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

            //entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();

        }

    }
    
}