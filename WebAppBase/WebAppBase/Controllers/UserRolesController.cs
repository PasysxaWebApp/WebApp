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

namespace WebAppBase.Controllers
{
    public class UserRolesController : Controller
    {
        private AppIdentityDbContext db = new AppIdentityDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: UserRoles
        public ActionResult Index()
        {
            //var roles = UserManager.GetRoles()
            return View();
        }
    }
}