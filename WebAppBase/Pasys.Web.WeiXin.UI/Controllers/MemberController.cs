using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.WeiXin.UI.Controllers
{
    public class MemberController : WorkController
    {
        public ActionResult BindUser()
        {
            return View();
        }

        // GET: Member
        public ActionResult Index()
        {
            return View();
        }
    }
}