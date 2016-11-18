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

namespace WebAppBase.Controllers
{
    public class RoleMenusController : Controller
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
                     Value=item.Name,
                     Text =item.Name
                });
            });
            mdl.RolesSelectList = roles;
            
            return View(mdl);
        }

        public ActionResult AjaxGetRoleMenu(string roleId)
        {
            var mdl = new RoleMenusViewModel();
            var lstMenu = new List<Models.SystemMenus.SystemMenuModel>();
            var lstRoleMenu = new List<Models.SystemMenus.SystemMenuModel>();
            var lstMenuTemp = new List<Models.SystemMenus.SystemMenuModel>();
            var lstRoleMenuTemp = new List<Models.SystemMenus.SystemMenuModel>();
            RoleMenuManager.GetMenus().Where(
                    witem=>!RoleMenuManager.GetMenusByRoleName(roleId).Select(s=>s.MenuId).Contains(witem.MenuId)
                    ).ToList().ForEach(item =>
            {
                lstMenuTemp.Add(new Models.SystemMenus.SystemMenuModel
                {
                    MenuID = item.MenuId,
                    MenuName = item.MenuName
                });
            });
            RoleMenuManager.GetMenusByRoleName(roleId).ForEach(item =>
            {
                lstRoleMenuTemp.Add(new Models.SystemMenus.SystemMenuModel
                {
                    MenuID = item.MenuId,
                    MenuName = item.MenuName
                });
            });
            mdl.MenuList = lstMenuTemp;
            mdl.RoleMenuList = lstRoleMenuTemp;
            return Json(mdl, JsonRequestBehavior.AllowGet);
        }
    }
}