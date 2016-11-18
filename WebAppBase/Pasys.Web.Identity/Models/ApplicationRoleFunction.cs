using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public interface IRoleFunctionStore : IRoleStore<ApplicationRole, string>
    {
        void CreateMenu(ApplicationFunction menu);
        List<ApplicationRole> GetAllowRolesByFunctionId(int FunctionId);
        List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName);
        Task<List<ApplicationFunction>> GetMenusByRoleNameAsync(string RoleName);
        List<ApplicationFunction> GetMenus();
        Task DeleteRoleMenus(string RoleName);

        Task AddRoleMenuAsync(string RoleId, int FunctionId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag);
        Task SetMenuDisplayNoAsync(string RoleId, int FunctionId, int DisplayNo);
        Task SetMenuAuthorizationStatusAsync(string RoleId, int FunctionId, int AuthorizationStatus);
    }


    public class ApplicationRoleFunction
    {
        //public int RoleFunctionId { get; set; }
        public int FunctionId { get; set; }
        public string RoleId { get; set; }
        public int AuthorizationStatus { get; set; }
        public int DisplayNo { get; set; }
        public bool ShowInMenu { get; set; }
        public bool SeparateMenuFlag { get; set; }
        //public ApplicationMenu Menu { get; set; }
        public ApplicationRole Role { get; set; }
        
    }

    public class RoleFunctionStore : RoleStore<ApplicationRole>, IRoleFunctionStore
    {
        public RoleFunctionStore(DbContext context)
            : base(context)
        {

        }

        public void CreateMenu(ApplicationFunction menu)
        {
            var MenuSet = Context.Set<ApplicationFunction>();
            var om = MenuSet.FirstOrDefault(m => m.FunctionId.Equals(menu.FunctionId));
            if (om == null)
            {
                MenuSet.Add(menu);
            }
            Context.SaveChanges();
        }

        public async Task<List<ApplicationFunction>> GetMenusByRoleNameAsync(string RoleName)
        {
            var role = await this.FindByNameAsync(RoleName);
            if (role == null)
            {
                return null;
            }

            var menus = Context.Set<ApplicationFunction>();
            List<ApplicationFunction> list = new List<ApplicationFunction>();
            role.RoleMenus.Sort((x, y) => x.DisplayNo.CompareTo(y.DisplayNo));
            foreach (var rm in role.RoleMenus)
            {
                var m = menus.Find(rm.FunctionId);
                if (m != null)
                {
                    m.ShowInMenu = rm.ShowInMenu;
                    m.SeparateMenuFlag = rm.SeparateMenuFlag;
                    list.Add(m);
                }
            }
            return list;
        }

        public List<ApplicationFunction> GetMenus()
        {
            var menus = Context.Set<ApplicationFunction>();
            return menus.ToList();
        }

        public async Task DeleteRoleMenus(string RoleName)
        {
            var role = await this.FindByNameAsync(RoleName);
            if (role == null)
            {
                return;
            }
            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            DbEntitySet.RemoveRange(role.RoleMenus);
            
            await Context.SaveChangesAsync();
        }

        public List<ApplicationRole> GetAllowRolesByFunctionId(int FunctionId)
        {
            var menus = Context.Set<ApplicationFunction>();
            var menu = menus.Find(FunctionId);
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }
            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            var roles = DbEntitySet.Where(m => m.FunctionId.Equals(FunctionId) && m.AuthorizationStatus == Convert.ToInt32(Authorization.Allow)).Select(m => m.Role).ToList();
            return roles;
        }
        public List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName)
        {
            var menus = Context.Set<ApplicationFunction>();
            var menu = menus.FirstOrDefault(m => m.ControllerName.ToLower().Equals(controllerName) && m.ActionName.ToLower().Equals(actionName));
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }

            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            var allowStatus = Convert.ToInt32(Authorization.Allow);
            var roles = DbEntitySet.Where(m => m.FunctionId.Equals(menu.FunctionId) && m.AuthorizationStatus == allowStatus).Select(m => m.Role).ToList();
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
            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.FunctionId.Equals(MenuId));
            if (roleMenu == null)
            {
                ApplicationRoleFunction rm = new ApplicationRoleFunction { FunctionId = MenuId, RoleId = RoleId, DisplayNo = DisplayNo ?? MenuId, AuthorizationStatus = Convert.ToInt32(Authorization.Allow), ShowInMenu = ShowInMenu, SeparateMenuFlag = SperateMenuFlag };
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

            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.FunctionId.Equals(MenuId));
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

            var DbEntitySet = Context.Set<ApplicationRoleFunction>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.FunctionId.Equals(MenuId));
            if (roleMenu != null)
            {
                roleMenu.AuthorizationStatus = AuthorizationStatus;
                Context.Entry(roleMenu).State = EntityState.Modified;
            }
            await Context.SaveChangesAsync();
            return;
        }


    }

    public class RoleFunctionManager : RoleManager<ApplicationRole>
    {
        public RoleFunctionManager(IRoleFunctionStore store) : base(store) { }

        protected new IRoleFunctionStore Store
        {
            get
            {
                return (IRoleFunctionStore)base.Store;
            }
        }

    }


}
