using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pasys.Web.Admin.UI.MvcLibrary;

namespace Pasys.Web.Admin.UI.Controllers
{
    [AutoRoleAuthorize]
    public class PriceAdjustController : Controller
    {
        // GET: PriceAdjust
        public ActionResult Index()
        {
            return new ContentResult() { Content = "Index" };
        }

        public ActionResult Edit()
        {
            return new ContentResult() { Content = "Edit" };
        }

        public ActionResult Test()
        {
            return new ContentResult() { Content = "Test" };
        }

    }
}