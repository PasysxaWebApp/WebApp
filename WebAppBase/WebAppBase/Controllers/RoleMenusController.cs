using Pasys.Web.Identity;
using Pasys.Web.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAppBase.Models;
using WebAppBase.MvcLibrary;

namespace WebAppBase.Controllers
{
    [AutoRoleAuthorize]
    public class RoleMenusController : WorkController
    {
        private AppIdentityDbContext db = new AppIdentityDbContext();

        private ApplicationRoleMenuManager _roleMenuManager;
        public ApplicationRoleMenuManager RoleMenuManager
        {
            get
            {
                return _roleMenuManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleMenuManager>();
            }
            private set
            {
                _roleMenuManager = value;
            }
        }
        // GET: RoleMenus
        public ActionResult Index()
        {
            var mdl = new RoleMenusViewModel();
            var roles = new List<SelectListItem>();
            roles.Add(new SelectListItem
            {
                 Value="",
                 Text = "",
                 Selected = true
            });
            db.Roles.ToList().ForEach(item =>
            {
                roles.Add(new SelectListItem
                {
                     Value=item.Id,
                     Text =item.Name
                });
            });
            mdl.RolesSelectList = roles;
            
            return View(mdl);
        }

        public ActionResult AjaxGetRoleMenu(string roleName)
        {
            var mdl = new RoleMenusViewModel();
            
            //RoleMenuManager.GetMenus().Where(
            //        witem => !RoleMenuManager.GetMenusByRoleName(roleName).Select(s => s.MenuId).Contains(witem.MenuId)
            //        ).ToList().ForEach(item =>
            RoleMenuManager.GetMenus().ToList().ForEach(item =>
            {
                mdl.MenuList.Add(new ApplicationFunction
                {
                    FunctionId = item.FunctionId,
                    MenuName = item.MenuName
                });
            });
            RoleMenuManager.GetMenusByRoleName(roleName).ForEach(item =>
            {
                mdl.RoleMenuList.Add(new ApplicationFunction
                {
                    FunctionId = item.FunctionId,
                    MenuName = item.MenuName
                });
            });
            return Json(mdl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxUpdateRoleMenu(RoleMenusViewModel mdl)
        {
            var roleMenus = mdl.RoleMenuList;
            RoleMenuManager.DeleteRoleMenus(mdl.Role);
            roleMenus.ForEach(item =>
            {
                RoleMenuManager.AddRoleMenu(mdl.Role, item.FunctionId, 1, true, false);
            });
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}