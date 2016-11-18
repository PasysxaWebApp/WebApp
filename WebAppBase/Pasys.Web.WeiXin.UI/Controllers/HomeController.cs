using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.WeiXin.UI.Controllers
{
    public class HomeController : WorkController
    {
        public ActionResult Index()
        {
            ViewBag.Cookies = this.Request.Cookies;
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
    }
}