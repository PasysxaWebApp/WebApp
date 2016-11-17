using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public interface IRoleMenuStore : IRoleStore<ApplicationRole, string>
    {
        void CreateMenu(ApplicationMenu menu);
        List<ApplicationRole> GetAllowRolesByMenuId(int MenuId);
        List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName);
        Task<List<ApplicationMenu>> GetMenusByRoleNameAsync(string RoleName);
        List<ApplicationMenu> GetMenus();
        Task AddRoleMenuAsync(string RoleId, int MenuId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag);
        Task SetMenuDisplayNoAsync(string RoleId, int MenuId, int DisplayNo);
        Task SetMenuAuthorizationStatusAsync(string RoleId, int MenuId, int AuthorizationStatus);
    }


    public class ApplicationRoleMenu
    {
        //public int RoleMenuId { get; set; }
        public int MenuId { get; set; }
        public string RoleId { get; set; }
        public int AuthorizationStatus { get; set; }
        public int DisplayNo { get; set; }
        public bool ShowInMenu { get; set; }
        public bool SeparateMenuFlag { get; set; }
        //public ApplicationMenu Menu { get; set; }
        public ApplicationRole Role { get; set; }
        
    }

    public class RoleMenuStore : RoleStore<ApplicationRole>, IRoleMenuStore
    {
        public RoleMenuStore(DbContext context)
            : base(context)
        {

        }

        public void CreateMenu(ApplicationMenu menu)
        {
            var MenuSet = Context.Set<ApplicationMenu>();
            var om = MenuSet.FirstOrDefault(m => m.MenuId.Equals(menu.MenuId));
            if (om != null)
            {
                throw new ArgumentException(string.Format("{0}", menu.MenuId));
            }
            MenuSet.Add(menu);
            Context.SaveChanges();
        }

        public async Task<List<ApplicationMenu>> GetMenusByRoleNameAsync(string RoleName)
        {
            var role = await this.FindByNameAsync(RoleName);
            if (role == null)
            {
                return null;
            }

            var menus = Context.Set<ApplicationMenu>();
            List<ApplicationMenu> list = new List<ApplicationMenu>();
            role.RoleMenus.Sort((x, y) => x.DisplayNo.CompareTo(y.DisplayNo));
            foreach (var rm in role.RoleMenus)
            {
                var m = menus.Find(rm.MenuId);
                if (m != null)
                {
                    m.ShowInMenu = rm.ShowInMenu;
                    m.SeparateMenuFlag = rm.SeparateMenuFlag;
                    list.Add(m);
                }
            }
            return list;
        }

        public List<ApplicationMenu> GetMenus()
        {
            var menus = Context.Set<ApplicationMenu>();
            return menus.ToList();
        }

        public List<ApplicationRole> GetAllowRolesByMenuId(int MenuId)
        {
            var menus = Context.Set<ApplicationMenu>();
            var menu = menus.Find(MenuId);
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }
            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roles = DbEntitySet.Where(m => m.MenuId.Equals(MenuId) && m.AuthorizationStatus == Convert.ToInt32(Authorization.Allow)).Select(m => m.Role).ToList();
            return roles;
        }
        public List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName)
        {
            var menus = Context.Set<ApplicationMenu>();
            var menu = menus.FirstOrDefault(m => m.ControllerName.ToLower().Equals(controllerName) && m.ActionName.ToLower().Equals(actionName));
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }

            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var allowStatus = Convert.ToInt32(Authorization.Allow);
            var roles = DbEntitySet.Where(m => m.MenuId.Equals(menu.MenuId) && m.AuthorizationStatus == allowStatus).Select(m => m.Role).ToList();
            return roles;

        }

        public async Task AddRoleMenuAsync(string RoleId, int MenuId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }
            //var roleMenu = await DbEntitySet.FindAsync(RoleId, MenuId);
            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu == null)
            {
                ApplicationRoleMenu rm = new ApplicationRoleMenu { MenuId = MenuId, RoleId = RoleId, DisplayNo = DisplayNo ?? MenuId, AuthorizationStatus = Convert.ToInt32(Authorization.Allow), ShowInMenu = ShowInMenu, SeparateMenuFlag = SperateMenuFlag };
                DbEntitySet.Add(rm);
            }
            await Context.SaveChangesAsync();
            return;
        }

        public async Task SetMenuDisplayNoAsync(string RoleId, int MenuId, int DisplayNo)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }

            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu != null)
            {
                roleMenu.DisplayNo = DisplayNo;
                Context.Entry(roleMenu).State = EntityState.Modified;
            }
            await Context.SaveChangesAsync();
            return;

        }

        public async Task SetMenuAuthorizationStatusAsync(string RoleId, int MenuId, int AuthorizationStatus)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }

            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu != null)
            {
                roleMenu.AuthorizationStatus = AuthorizationStatus;
                Context.Entry(roleMenu).State = EntityState.Modified;
            }
            await Context.SaveChangesAsync();
            return;
        }


    }

    public class RoleMenuManager : RoleManager<ApplicationRole>
    {
        public RoleMenuManager(IRoleMenuStore store) : base(store) { }

        protected new IRoleMenuStore Store
        {
            get
            {
                return (IRoleMenuStore)base.Store;
            }
        }

    }


}
