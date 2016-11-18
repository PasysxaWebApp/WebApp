using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Pasys.Web.Identity.Models
{
    public class AppIdentityDbInitializer : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public override void InitializeDatabase(AppIdentityDbContext context)
        {
            base.InitializeDatabase(context);
        }

        //创建用户名为admin@123.com，密码为“Admin@123456”并把该用户添加到角色组"Admin"中
        public static void InitializeIdentityForEF(AppIdentityDbContext db)
        {
            /*             
                drop table [dbo].[__MigrationHistory];
                drop table [dbo].[account_m_rolemenus];
                drop table [dbo].[account_m_userroles];
                drop table [dbo].[account_m_userclaims];
                drop table [dbo].[account_m_userlogins];
                drop table [dbo].[account_m_users];
                drop table [dbo].[account_m_menus];
                drop table [dbo].[account_m_roles];
                drop table [dbo].[account_m_organizations];
             */

            
            var org = new ApplicationOrganization()
            {
                OrganizationId = "DebugOrganizationID",
                OrganizationCode = "ABC",
                OfficeName = "事業所名 0001",
                StaffName = "責任者名001",
                Postal = "798-3332",
                Address1 = "愛媛県宇和島市津島町須下362",
                Address2 = "",
                MailAddress = "yangyang@pasysxa.com",
                PhoneNumber = "1236634",
                FaxNumber = "1236634",
                LastUserID = "SYS",
                LastUpdatetime = System.DateTime.Now
            };
            var orgManager = new OrganizationManager(new OrganizationStore(db));
            orgManager.Create(org);

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db)); //HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = new ApplicationRoleMenuManager(new RoleMenuStore(db)); // HttpContext.Current.GetOwinContext().Get<ApplicationRoleMenuManager>();

            const string ognUserName = "ognauser@123.com";//用户名
            const string ognAdminName = "ognadmin@123.com";//用户名
            const string adminName = "admin@123.com";//用户名
            const string password = "Admin@123456";//密码

            //0:閲覧のみ　1:利用者　2:管理者 3:システム管理者
            const string VisitorName = "Visitor";//用户要添加到的角色组
            const string OrganizationUserName = "OrganizationUserName";//用户要添加到的角色组
            const string OrganizationAdminName = "OrganizationAdmin";//用户要添加到的角色组
            const string AdminName = "Admin";//用户要添加到的角色组

            //如果没有Admin用户组则创建该组
            var roleVisitor = roleManager.FindByName(VisitorName);
            if (roleVisitor == null)
            {
                roleVisitor = new ApplicationRole(org.OrganizationId, VisitorName, "閲覧のみ");
                var roleresult = roleManager.Create(roleVisitor);
            }
            var roleOgnUser = roleManager.FindByName(OrganizationUserName);
            if (roleOgnUser == null)
            {
                roleOgnUser = new ApplicationRole(org.OrganizationId, OrganizationUserName, "利用者");
                var roleresult = roleManager.Create(roleOgnUser);
            }
            var roleOgnAdmin = roleManager.FindByName(OrganizationAdminName);
            if (roleOgnAdmin == null)
            {
                roleOgnAdmin = new ApplicationRole(org.OrganizationId, OrganizationAdminName, "管理者");
                var roleresult = roleManager.Create(roleOgnAdmin);
            }
            var roleAdmin = roleManager.FindByName(AdminName);
            if (roleAdmin == null)
            {
                roleAdmin = new ApplicationRole(org.OrganizationId, AdminName, "システム管理者");
                var roleresult = roleManager.Create(roleAdmin);
            }
            //===============================================
            //如果没有admin@123.com用户则创建该用户
            var user = userManager.FindByName(adminName);
            if (user == null)
            {
                user = new ApplicationUser { OrganizationId = org.OrganizationId, UserName = adminName, Email = adminName };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // 把用户admin@123.com添加到用户组Admin中
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleAdmin.Name))
            {
                var result = userManager.AddToRole(user.Id, roleAdmin.Name);
            }

            //===============================================
            //如果没有ognadmin@123.com用户则创建该用户
            user = userManager.FindByName(ognAdminName);
            if (user == null)
            {
                user = new ApplicationUser { OrganizationId = org.OrganizationId, UserName = ognAdminName, Email = ognAdminName };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // 把用户ognAdminName添加到用户组OrganizationAdmin中
            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleOgnAdmin.Name))
            {
                var result = userManager.AddToRole(user.Id, roleOgnAdmin.Name);
            }

            //===============================================
            //如果没有ognauser@123.com用户则创建该用户
            user = userManager.FindByName(ognUserName);
            if (user == null)
            {
                user = new ApplicationUser { OrganizationId = org.OrganizationId, UserName = ognUserName, Email = ognUserName };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // 把用户ognUserName添加到用户组OrganizationUserName中
            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(roleOgnUser.Name))
            {
                var result = userManager.AddToRole(user.Id, roleOgnUser.Name);
            }


            var m = new ApplicationMenu { MenuId = 1, ParentMenuId = 1, MenuName = "ホーム", ActionName = "Index", ControllerName = "Home", CssClass = "icons_24_home", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 2, ParentMenuId = 2, MenuName = "記録", ActionName = "", ControllerName = "", CssClass = "icons_24_record", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 3, ParentMenuId = 2, MenuName = "飼育記録", ActionName = "FarmRecodeList", ControllerName = "FarmRecode", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 4, ParentMenuId = 2, MenuName = "単価変更", ActionName = "Index", ControllerName = "PriceAdjust", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 15, ParentMenuId = 2, MenuName = "単価変更", ActionName = "Edit", ControllerName = "PriceAdjust", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 5, ParentMenuId = 5, MenuName = "集計", ActionName = "", ControllerName = "", CssClass = "icons_24_sum", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 6, ParentMenuId = 5, MenuName = "生簀一覧（日次）", ActionName = "FishSpeciesList", ControllerName = "FishSpeciesList", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 7, ParentMenuId = 5, MenuName = "生簀一覧（月次）", ActionName = "FishSpeciesListMonth", ControllerName = "FishSpeciesListMonth", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 8, ParentMenuId = 8, MenuName = "特殊", ActionName = "", ControllerName = "", CssClass = "icons_24_spe", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 9, ParentMenuId = 8, MenuName = "分割一覧", ActionName = "Index", ControllerName = "Division", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 10, ParentMenuId = 8, MenuName = "災害一覧", ActionName = "Index", ControllerName = "Disaster", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 11, ParentMenuId = 11, MenuName = "設定", ActionName = "", ControllerName = "", CssClass = "icons_24_setting", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 12, ParentMenuId = 11, MenuName = "漁場設定", ActionName = "FishingGroundList", ControllerName = "FishingGround", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 13, ParentMenuId = 11, MenuName = "生簀分類設定", ActionName = "FishSpeciesGroupList", ControllerName = "FishSpeciesGroup", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 14, ParentMenuId = 11, MenuName = "生簀設定", ActionName = "FishSpeciesList", ControllerName = "FishSpecies", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 15, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 16, ParentMenuId = 11, MenuName = "環境情報設定", ActionName = "EnvironmentList", ControllerName = "Environment", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 17, ParentMenuId = 11, MenuName = "チェック項目設定", ActionName = "CheckItemList", ControllerName = "CheckItem", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 18, ParentMenuId = 11, MenuName = "作業項目設定", ActionName = "WorkItemList", ControllerName = "WorkItem", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 19, ParentMenuId = 11, MenuName = "給餌状況設定", ActionName = "BaitStatusList", ControllerName = "BaitStatus", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 20, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 21, ParentMenuId = 11, MenuName = "魚種設定", ActionName = "FishKindList", ControllerName = "FishKind", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 22, ParentMenuId = 11, MenuName = "斃死理由設定", ActionName = "DeadReasonList", ControllerName = "DeadReason", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 23, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 24, ParentMenuId = 11, MenuName = "種苗導入記録", ActionName = "SeedIntroRecodeList", ControllerName = "SeedIntroRecode", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 25, ParentMenuId = 11, MenuName = "仕入先設定", ActionName = "StockToList", ControllerName = "StockTo", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 26, ParentMenuId = 11, MenuName = "出荷先設定", ActionName = "ShipmentToList", ControllerName = "ShipmentTo", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 27, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 28, ParentMenuId = 11, MenuName = "飼料設定", ActionName = "FeedList", ControllerName = "Feed", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 29, ParentMenuId = 11, MenuName = "栄養剤設定", ActionName = "NutrientList", ControllerName = "Nutrient", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 30, ParentMenuId = 11, MenuName = "薬設定", ActionName = "MedicineList", ControllerName = "Medicine", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 31, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 32, ParentMenuId = 11, MenuName = "ユーザー招待管理", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 33, ParentMenuId = 33, MenuName = "PASYS設定", ActionName = "", ControllerName = "", CssClass = "icons_24_setting", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 34, ParentMenuId = 33, MenuName = "事業所設定", ActionName = "Index", ControllerName = "Organization", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 35, ParentMenuId = 33, MenuName = "ユーザー管理", ActionName = "UserList", ControllerName = "User", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 36, ParentMenuId = 33, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 37, ParentMenuId = 11, MenuName = "飼料区分設定", ActionName = "FeedDivList", ControllerName = "FeedDiv", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 38, ParentMenuId = 33, MenuName = "投与物計上設定", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 39, ParentMenuId = 33, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 40, ParentMenuId = 33, MenuName = "種苗種類設定", ActionName = "SeedKindList", ControllerName = "SeedKind", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 41, ParentMenuId = 33, MenuName = "生簀計上設定", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 42, ParentMenuId = 33, MenuName = "網種類設定", ActionName = "NetList", ControllerName = "Net", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 43, ParentMenuId = 33, MenuName = "共通魚種設定", ActionName = "PublicFishKindList", ControllerName = "PublicFishKind", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 44, ParentMenuId = 33, MenuName = "輸送方法設定", ActionName = "TransportList", ControllerName = "Transport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 45, ParentMenuId = 33, MenuName = "種苗由来設定", ActionName = "SeedOriginList", ControllerName = "SeedOrigin", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 46, ParentMenuId = 46, MenuName = "レポート", ActionName = "", ControllerName = "", CssClass = "icons_24_report", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 47, ParentMenuId = 46, MenuName = "斃死レポート", ActionName = "DeadReportList", ControllerName = "DeadReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 48, ParentMenuId = 46, MenuName = "チェックレポート", ActionName = "CheckReportList", ControllerName = "CheckReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 49, ParentMenuId = 46, MenuName = "作業レポート", ActionName = "WorkReportList", ControllerName = "WorkReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 50, ParentMenuId = 46, MenuName = "出荷レポート", ActionName = "ShipmentReportList", ControllerName = "ShipmentReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 51, ParentMenuId = 46, MenuName = "生簀情報CSVレポート", ActionName = "FishSpeciesReportList", ControllerName = "FishSpeciesReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 52, ParentMenuId = 46, MenuName = "月次レポート", ActionName = "MonthReportList", ControllerName = "MonthReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 53, ParentMenuId = 46, MenuName = "投与物レポート", ActionName = "MortalityReportsList", ControllerName = "MortalityReports", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 54, ParentMenuId = 46, MenuName = "日々レポート", ActionName = "DayReportList", ControllerName = "DayReport", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 55, ParentMenuId = 33, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 56, ParentMenuId = 33, MenuName = "エラーログ", ActionName = "ErrorLogList", ControllerName = "ErrorLog", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 57, ParentMenuId = 33, MenuName = "アクセスログ", ActionName = "AccessLogList", ControllerName = "AccessLog", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 58, ParentMenuId = 33, MenuName = "システム利用状況", ActionName = "UseSituationList", ControllerName = "UseSituation", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 59, ParentMenuId = 33, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 60, ParentMenuId = 33, MenuName = "システム管理項目マスタ", ActionName = "Index", ControllerName = "SystemSetting", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 61, ParentMenuId = 2, MenuName = "飼育記録", ActionName = "BatchSave", ControllerName = "FishSpecieStatus", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 62, ParentMenuId = 11, MenuName = "間接経費科目一覧", ActionName = "Index", ControllerName = "AccountsTitle", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 63, ParentMenuId = 2, MenuName = "間接経費一覧", ActionName = "Index", ControllerName = "IndirectCost", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 64, ParentMenuId = 11, MenuName = "事業所設定", ActionName = "Index", ControllerName = "Organization", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 65, ParentMenuId = 11, MenuName = "ユーザー管理", ActionName = "Index", ControllerName = "UserManage", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            //m = new ApplicationMenu { MenuId = 66, ParentMenuId = 11, MenuName = "--", ActionName = "", ControllerName = "", CssClass = "", ActionParam = "" };
            //roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 67, ParentMenuId = 11, MenuName = "間接経費科目設定", ActionName = "Index", ControllerName = "AccountsTitle", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 68, ParentMenuId = 11, MenuName = "調整理由", ActionName = "AdjustReasonList", ControllerName = "AdjustReason", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);
            m = new ApplicationMenu { MenuId = 69, ParentMenuId = 33, MenuName = "お知らせ", ActionName = "Index", ControllerName = "Notice", CssClass = "", ActionParam = "" };
            roleManager.CreateMenu(m);

            if (roleVisitor != null)
            {
                roleManager.AddRoleMenu(roleVisitor.Id, 1, 0, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 5, 5, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 6, 6, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 7, 7, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 46, 46, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 47, 47, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 48, 48, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 49, 49, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 50, 50, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 51, 51, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 52, 52, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 53, 53, true, false);
                roleManager.AddRoleMenu(roleVisitor.Id, 54, 54, true, false);
            }

            if (roleOgnUser != null)
            {
                roleManager.AddRoleMenu(roleOgnUser.Id, 1, 0, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 2, 100, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 4, 102, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 5, 200, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 6, 201, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 7, 202, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 8, 300, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 9, 301, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 10, 302, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 11, 500, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 12, 501, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 13, 502, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 14, 503, true, false);
                //roleManager.AddRoleMenu(roleOgnUser.Id, 15, 504, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 16, 505, true, true);
                roleManager.AddRoleMenu(roleOgnUser.Id, 17, 506, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 18, 507, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 19, 508, true, false);
                //roleManager.AddRoleMenu(roleOgnUser.Id, 20, 509, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 21, 510, true, true);
                roleManager.AddRoleMenu(roleOgnUser.Id, 22, 511, true, false);
                //roleManager.AddRoleMenu(roleOgnUser.Id, 23, 512, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 24, 502, true, true);
                roleManager.AddRoleMenu(roleOgnUser.Id, 25, 514, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 26, 515, true, false);
                //roleManager.AddRoleMenu(roleOgnUser.Id, 27, 516, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 28, 518, true, true);
                roleManager.AddRoleMenu(roleOgnUser.Id, 29, 519, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 30, 520, true, false);
                //roleManager.AddRoleMenu(roleOgnUser.Id, 31, 521, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 37, 517, true, true);
                roleManager.AddRoleMenu(roleOgnUser.Id, 46, 400, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 47, 401, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 48, 402, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 49, 403, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 50, 405, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 51, 405, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 52, 406, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 53, 407, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 54, 408, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 61, 101, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 63, 104, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 67, 522, true, false);
                roleManager.AddRoleMenu(roleOgnUser.Id, 68, 523, true, false);
            }

            if (roleOgnAdmin != null)
            {
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 1, 0, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 2, 100, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 4, 102, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 5, 200, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 6, 201, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 7, 202, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 8, 300, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 9, 301, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 10, 302, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 11, 500, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 12, 505, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 13, 505, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 14, 507, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 15, 507, false, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 16, 507, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 17, 507, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 18, 507, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 19, 508, true, false);
                //roleManager.AddRoleMenu(roleOgnAdmin.Id, 20, 509, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 21, 510, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 22, 511, true, false);
                //roleManager.AddRoleMenu(roleOgnAdmin.Id, 23, 512, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 24, 506, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 25, 514, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 26, 515, true, false);
                //roleManager.AddRoleMenu(roleOgnAdmin.Id, 27, 516, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 28, 517, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 29, 518, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 30, 519, true, false);
                //roleManager.AddRoleMenu(roleOgnAdmin.Id, 31, 520, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 37, 516, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 46, 400, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 47, 401, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 48, 402, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 49, 403, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 50, 404, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 51, 405, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 52, 406, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 53, 407, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 54, 408, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 61, 101, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 63, 104, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 64, 501, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 65, 502, true, false);
                //roleManager.AddRoleMenu(roleOgnAdmin.Id, 66, 503, true, false);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 67, 521, true, true);
                roleManager.AddRoleMenu(roleOgnAdmin.Id, 68, 522, true, false);

            }

            if (roleAdmin != null)
            {
                roleManager.AddRoleMenu(roleAdmin.Id, 1, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 33, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 34, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 35, 0, true, false);
                //roleManager.AddRoleMenu(roleAdmin.Id, 39, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 40, 0, true, true);
                roleManager.AddRoleMenu(roleAdmin.Id, 42, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 43, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 44, 0, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 45, 0, true, false);
                //roleManager.AddRoleMenu(roleAdmin.Id, 55, 203, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 56, 204, true, true);
                roleManager.AddRoleMenu(roleAdmin.Id, 57, 205, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 58, 206, true, false);
                roleManager.AddRoleMenu(roleAdmin.Id, 69, 0, true, false);
            }


        }
    }
}
