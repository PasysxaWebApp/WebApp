using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pasys.Web.Admin.UI.Configs;
using Pasys.Web.Admin.UI.Models.SystemMenus;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Pasys.Web.Identity;
using System.Text;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class HomeController : WorkController
    {
        public HomeController()
        {
        }


        public HomeController(ApplicationUserManager userManager,
            ApplicationRoleMenuManager roleManager):base(userManager)
        {
            RoleManager = roleManager;
        }

        private ApplicationRoleMenuManager _roleManager;
        public ApplicationRoleMenuManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleMenuManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            //var at = "OrganizationAdmin";            
            //var menus=RoleManager.GetMenusByRoleName(at);
            var aes= SharedUtilitys.Helper.SecureHelper.GetWequence(64);
            var sha1 = SharedUtilitys.Helper.SecureHelper.GetWequence(128);
            ViewBag.AES = aes;
            ViewBag.SHA1 = sha1;
           
            ViewBag.Cookies =this.Request.Cookies;            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            try
            {
                if (Request.UserHostAddress != null)
                {
                    var ex = (Exception)HttpContext.Application[Request.UserHostAddress];

                    if (ex != null)
                    {
                        //var loginInfoSession = SessionLoginInfo.GetInstance(Session);
                        SystemLogManager.GetInstance().SetSystemErrorLog(this.WorkContext.GlobalConfig.SiteName, this.WorkContext.UserInfo.OrganizationId, this.WorkContext.UserId, this.WorkContext.UserName, ex.Message, ex.StackTrace);
                    }
                }
            }
            catch (Exception exx)
            {
                Debug.WriteLine(exx.Message);
            }

            return View("Error");
        }

        public ActionResult Http404()
        {
            return View("Error");
        }

        public ActionResult TimeoutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult RolloutRedirect(string OrganizationID)
        {
            ViewBag.OrganizationID = string.Format("{0}", OrganizationID);
            return View();
        }

        public ActionResult Live()
        {
            return new EmptyResult();
        }

        public ActionResult GetMenuHTML()
        {
            var at = this.WorkContext.GlobalConfig.DefaultRole;
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var roles = UserManager.GetRoles(User.Identity.GetUserId());
                if (roles.Count > 0)
                {
                    at = roles[0];
                }
            }
            var menus =  RoleManager.GetMenusByRoleName(at);
            var menuHtml = SystemMenuListModel.GetInstance().GetResponsiveHtml(menus, Url);
            return Content(menuHtml);
        }

        public ActionResult GetBootStrapMenuHTML()
        {
            var at = this.WorkContext.GlobalConfig.DefaultRole;
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var roles = UserManager.GetRoles(User.Identity.GetUserId());
                if (roles.Count > 0)
                {
                    at = roles[0];
                }
            }
            var menus =  RoleManager.GetMenusByRoleName(at);
            var menuHtml = SystemMenuListModel.GetInstance().GetBootStrapHtml(menus, Url);
            return Content(menuHtml);
        }

    }
}