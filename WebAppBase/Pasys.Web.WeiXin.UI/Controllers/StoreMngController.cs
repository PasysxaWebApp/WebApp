using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.WeiXin.UI.Controllers
{
    public class StoreMngController : Controller
    {
        // GET: StoreMng
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpStore() {
            return View();
        }
    }
}