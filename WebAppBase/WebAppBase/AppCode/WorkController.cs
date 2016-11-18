using Pasys.Web.Core;
using SharedUtilitys.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Pasys.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;

namespace WebAppBase
{
    public class WorkController : Controller, IWorkController<WorkContext>
    {
        private ApplicationUserManager _userManager;
        private WorkContext _workContext = new WorkContext();

        public WorkContext WorkContext
        {
            get {
                return _workContext;
            }
        }       

        public WorkController()
        { 
        }

        public WorkController(ApplicationUserManager userManager)
            : this()
        {
            UserManager = userManager;
        }


        public virtual ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //Reqeust
            _workContext.IsHttpAjax = WebHelper.IsAjax();
            _workContext.IP = WebHelper.GetIP();
            _workContext.Url = WebHelper.GetUrl();
            _workContext.UrlReferrer = WebHelper.GetUrlReferrer();
            //UserInfo
            _workContext.UserId = requestContext.HttpContext.User.Identity.GetUserId();
            _workContext.UserName = requestContext.HttpContext.User.Identity.GetUserName();
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _workContext.UserInfo = UserManager.FindById(_workContext.UserId);
            }
            
            //当前控制器类名
            _workContext.Controller = requestContext.RouteData.Values["controller"].ToString().ToLower();
            //当前动作方法名
            _workContext.Action = RouteData.Values["action"].ToString().ToLower();
            _workContext.PageKey = string.Format("/{0}/{1}", _workContext.Controller, _workContext.Action);

        }

    }
}
