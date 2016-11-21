using Pasys.Web.Core;
using SharedUtilitys.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Pasys.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;
using Senparc.Weixin.MP.Containers;
using Pasys.Web.WeiXin.UI.Utility;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin;
using System.Security.Claims;

namespace Pasys.Web.WeiXin.UI
{
    public class WorkController : Controller, IWorkController<WorkContext>
    {
        private ApplicationUserManager _userManager;
        private WorkContext _workContext = new WorkContext();

        public WorkContext WorkContext
        {
            get
            {
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

        public IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            /*
            #region weixin

            ExternalLoginInfo loginInfo = AuthManager.GetExternalLoginInfo();
            var user =  UserManager.Find(loginInfo.Login);

            var code = WebHelper.GetQueryString("code");
            var state = WebHelper.GetQueryString("state");
            var openId = WebUtils.GetCookie("openid");

            if (string.IsNullOrWhiteSpace(openId) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state) && state == "lxj")
            {
                //通过，用code换取access_token
                var config = this._workContext.WeiXinMPConfig;
                string appId = config.WeixinAppId;
                string appSecret = config.WeixinAppSecret;
                Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = null;
                try
                {
                    result = OAuthApi.GetAccessToken(appId, appSecret, code);
                }
                catch (Exception)
                { }
                if (result != null && result.errcode == ReturnCode.请求成功)
                {
                    _workContext.openId = result.openid;
                }
                else
                {
                    _workContext.openId = string.Empty;// (string)requestContext.HttpContext.Session["openId"];
                }
                WebUtils.SetCookie("openid", _workContext.openId);
            }

            //测试用
#if DEBUG
            if (string.IsNullOrWhiteSpace(_workContext.openId) && Request.Url.Host.ToLower().Equals("localhost"))
            {
                _workContext.openId = "ozZZ5t_VheKVfHlv03srm6ylieyU";
                WebUtils.SetCookie("openid", _workContext.openId);
            }
#endif

            if (_workContext.UserInfo == null)
            {                
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, _workContext.openId));
                var identity= new ClaimsIdentity(claims,"weixin");
                 
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                UserManager.AddClaim("",new System.Security.Claims.Claim("weixin",_workContext.UserId));
            }

            #endregion
            */
            #region workcontext
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
            #endregion


        }

        public string GetToken()
        {
            try
            {
                var config = this.WorkContext.WeiXinMPConfig;
                string appId = config.WeixinAppId;
                string appSecret = config.WeixinAppSecret;
                var access_token = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(appId, appSecret);
                //return access_token.access_token;// result.access_token;// Json(result, JsonRequestBehavior.AllowGet);
                //if (!AccessTokenContainer.CheckRegistered(appId))
                //{
                //    AccessTokenContainer.Register(appId, appSecret);
                //}
                //var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(appId, appSecret);//AccessTokenContainer.GetTokenResult(appId);                

                //也可以直接一步到位：
                return AccessTokenContainer.TryGetAccessToken(appId, appSecret);
            }
            catch (Exception ex)
            {
                //TODO:为简化代码，这里不处理异常（如Token过期）
                throw ex;// Json(new { error = "执行过程发生错误！" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }

    public static class UrlExtensions
    {
        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="contentPath"></param>
        /// <returns></returns>
        public static string AreaContent(this UrlHelper Url, string contentPath)
        {
            if (contentPath.StartsWith("~") && !string.IsNullOrEmpty(WorkContext.GLOBAL_CONFIG.WorkContextArea))
            {
                contentPath = string.Format("~/{0}/{1}", WorkContext.GLOBAL_CONFIG.WorkContextArea, contentPath.Substring(2));
            }
            return Url.Content(contentPath);
        }
    }

}
