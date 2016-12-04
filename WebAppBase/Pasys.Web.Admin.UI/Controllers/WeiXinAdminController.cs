using Pasys.Web.Admin.UI.Models;
using System.Web.Mvc;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class WeiXinAdminController : Controller
    {
        // GET: WeiXinAdmin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNew()
        {
            return View();
        }

    }
}