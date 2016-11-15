using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Pasys.Web.Identity.Models;

namespace Pasys.Web.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 電子メールを送信するには、電子メール サービスをここにプラグインします。
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // テキスト メッセージを送信するための SMS サービスをここにプラグインします。
            return Task.FromResult(0);
        }
    }

    // このアプリケーションで使用されるアプリケーション ユーザー マネージャーを設定します。UserManager は ASP.NET Identity の中で定義されており、このアプリケーションで使用されます。
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {            
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // ユーザー名の検証ロジックを設定します
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // パスワードの検証ロジックを設定します
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // ユーザー ロックアウトの既定値を設定します。
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 2 要素認証プロバイダーを登録します。このアプリケーションでは、Phone and Emails をユーザー検証用コード受け取りのステップとして使用します。
            // 独自のプロバイダーをプログラミングしてここにプラグインできます。
            manager.RegisterTwoFactorProvider("電話コード", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "あなたのセキュリティ コードは {0} です。"
            });
            manager.RegisterTwoFactorProvider("電子メール コード", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "セキュリティ コード",
                BodyFormat = "あなたのセキュリティ コードは {0} です。"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // このアプリケーションで使用されるアプリケーション サインイン マネージャーを構成します。
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    //配置此应用程序中使用的应用程序角色管理器。RoleManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    //public class ApplicationRoleManager : RoleManager<ApplicationRole>
    //{
    //    public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
    //        : base(roleStore)
    //    {
    //    }

    //    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    //    {
    //        return new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
    //    }

    //}

    public class ApplicationRoleMenuManager : RoleMenuManager
    {
        public ApplicationRoleMenuManager(IRoleMenuStore roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleMenuManager Create(IdentityFactoryOptions<ApplicationRoleMenuManager> options, IOwinContext context)
        {
            return new ApplicationRoleMenuManager(new RoleMenuStore(context.Get<ApplicationDbContext>()));
        }
        public virtual void CreateMenu(ApplicationMenu menu)
        {
            this.Store.CreateMenu(menu);
        }
        public virtual async Task<List<ApplicationMenu>> GetMenusByRoleNameAsync(string RoleName)
        {
            return await this.Store.GetMenusByRoleNameAsync(RoleName);
        }

        public virtual  List<ApplicationMenu> GetMenusByRoleName(string RoleName)
        {
            var _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
            return _myTaskFactory.StartNew(() => { return this.Store.GetMenusByRoleNameAsync(RoleName); }).Unwrap().GetAwaiter().GetResult();
        }

        public virtual void AddRoleMenu(string RoleId, int MenuId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag)
        {
            //var task = this.Store.AddRoleMenuAsync(RoleId, MenuId, DisplayNo);
            var _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
            _myTaskFactory.StartNew(() => { return this.Store.AddRoleMenuAsync(RoleId, MenuId, DisplayNo, ShowInMenu, SperateMenuFlag); }).Unwrap().GetAwaiter().GetResult();
        }

        public virtual void SetRoleMenuAsync(string RoleId, int MenuId, Authorization auth)
        {
            this.Store.SetMenuAuthorizationStatusAsync(RoleId, MenuId, Convert.ToInt32(auth));
        }

        public virtual List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName)
        {
            try
            {
                return this.Store.GetAllowRolesByControllNameActionName(controllerName, actionName);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<ApplicationRole> GetAllowRolesByMenuId(int MenuId)
        {
            return this.Store.GetAllowRolesByMenuId(MenuId);
        }

    }



}
