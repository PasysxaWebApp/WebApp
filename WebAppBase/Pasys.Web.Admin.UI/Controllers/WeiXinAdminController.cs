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
            var model = new SampleModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateNew()
        {
            var model = new SampleModel();
            return View("Index", model);
        }

    }
}