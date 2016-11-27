using Pasys.Web.Core;
using Pasys.Core.Helper;
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
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Pasys.Web.Identity.Models;
using Pasys.Web.Core.EntityManager;

namespace Pasys.Web.WeiXin.UI
{
    public class WorkController : Controller, IWorkController<WorkContext>
    {
        private ApplicationSignInManager _signInManager;
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

        public WorkController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            : this()
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
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


            #region weixin

            //ExternalLoginInfo loginInfo = AuthManager.GetExternalLoginInfo();
            //var user =  UserManager.Find(loginInfo.Login);

            var code = WebHelper.GetQueryString("code");
            var state = WebHelper.GetQueryString("state");
            _workContext.openId = WebUtils.GetCookie("openid");
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state) && state == this.WorkContext.WeiXinMPConfig.AuthorizeState)
            {
                OAuthAccessTokenResult result = null;
                try
                {
                    result = GetOAuthAccessTokenResult(code);
                }
                catch (Exception)
                { }

                if (result != null && result.errcode == ReturnCode.请求成功)
                {
                    _workContext.openId = result.openid;
                }
                WebUtils.SetCookie("openid", _workContext.openId);

                var access_token = GetToken();
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(access_token, _workContext.openId);

                var wxUserInfoManager = new UserInfoManager();
                var wxUserInfo = wxUserInfoManager.FindById(_workContext.openId);
                if (wxUserInfo == null)
                {
                    wxUserInfo = new WeiXinUserInfo()
                    {
                        OrganizationId = _workContext.UserInfo.OrganizationId,
                    };
                    FillOAuthUserInfoToWxUserInfo(userInfo, wxUserInfo);
                    wxUserInfoManager.Create(wxUserInfo);
                }
                else
                {
                    FillOAuthUserInfoToWxUserInfo(userInfo, wxUserInfo);
                    wxUserInfoManager.Update(wxUserInfo);
                }
                this._workContext.WxUserInfo = wxUserInfo;
            }

            //测试用
#if DEBUG
            //if (string.IsNullOrWhiteSpace(_workContext.openId) && Request.Url.Host.ToLower().Equals("localhost"))
            //{
            //    _workContext.openId = "ozZZ5t_VheKVfHlv03srm6ylieyU";
            //    WebUtils.SetCookie("openid", _workContext.openId);
            //}
#endif
            if (this._workContext.WxUserInfo == null && !string.IsNullOrEmpty(_workContext.openId))
            {
                var wxUserInfoManager = new UserInfoManager();
                this._workContext.WxUserInfo = wxUserInfoManager.FindById(_workContext.openId);
            }
            //UserInfo
            if (!requestContext.HttpContext.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(_workContext.openId))
            {
                var bindMng = new UserBindManager();
                var userId = bindMng.GetUserId(_workContext.openId);
                if (!string.IsNullOrEmpty(userId))
                {
                    _workContext.UserInfo = UserManager.FindById(userId);
                    SignInManager.SignInAsync(_workContext.UserInfo, isPersistent: true, rememberBrowser: true);
                }
                else
                {
                    string randomEmail = string.Format("{0}@xh2005.com", Guid.NewGuid());
                    var user = new ApplicationUser { OrganizationId = "DebugOrganizationID", UserName = randomEmail, Email = randomEmail };
                    if (this._workContext.WxUserInfo != null) {
                        user.NiceName = this._workContext.WxUserInfo.NickName;
                    }
                    var result = UserManager.Create(user, Guid.NewGuid().ToString());
                    if (result.Succeeded)
                    {
                        bindMng.BindUser(user.Id, _workContext.openId);
                        SignInManager.SignIn(user, isPersistent: true, rememberBrowser: true);
                    }
                    _workContext.UserInfo = user;
                }

                //var access_token = GetToken();
                //OAuthUserInfo userInfo = OAuthApi.GetUserInfo(access_token, _workContext.openId);

                //var wxUserInfoManager = new UserInfoManager();
                //var wxUserInfo = wxUserInfoManager.FindById(_workContext.openId);
                //if (wxUserInfo == null)
                //{
                //    wxUserInfo = new WeiXinUserInfo()
                //    {
                //        OrganizationId = _workContext.UserInfo.OrganizationId,
                //    };
                //    FillOAuthUserInfoToWxUserInfo(userInfo, wxUserInfo);
                //    wxUserInfoManager.Create(wxUserInfo);
                //}
                //else {
                //    FillOAuthUserInfoToWxUserInfo(userInfo, wxUserInfo);
                //    wxUserInfoManager.Update(wxUserInfo);
                //}

                //var claims = new List<Claim>();
                //claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                //claims.Add(new Claim(ClaimTypes.Name, _workContext.UserInfo.UserName));
                //claims.Add(new Claim(ClaimTypes.Sid, _workContext.openId));
                //var identity = new ClaimsIdentity(claims, "weixin");                
                //var principal = new ClaimsPrincipal(identity);
                //requestContext.HttpContext.User = principal;
            }
            else if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _workContext.UserInfo = UserManager.FindByName(requestContext.HttpContext.User.Identity.Name);
                _workContext.UserId = requestContext.HttpContext.User.Identity.GetUserId();
                _workContext.UserName = requestContext.HttpContext.User.Identity.GetUserName();
                var bindMng = new UserBindManager();
                _workContext.openId = bindMng.GeOpenId(_workContext.UserId);
                WebUtils.SetCookie("openid", _workContext.openId);
            }


            #endregion

            #region workcontext
            //Reqeust
            _workContext.IsHttpAjax = WebHelper.IsAjax();
            _workContext.IP = WebHelper.GetIP();
            _workContext.Url = WebHelper.GetUrl();
            _workContext.UrlReferrer = WebHelper.GetUrlReferrer();

            //当前控制器类名
            _workContext.Controller = requestContext.RouteData.Values["controller"].ToString().ToLower();
            //当前动作方法名
            _workContext.Action = RouteData.Values["action"].ToString().ToLower();
            _workContext.PageKey = string.Format("/{0}/{1}", _workContext.Controller, _workContext.Action);
            #endregion


        }

        private void FillOAuthUserInfoToWxUserInfo(OAuthUserInfo userInfo ,WeiXinUserInfo wxUserInfo)
        {
            wxUserInfo.City = userInfo.city;
            wxUserInfo.Country = userInfo.country;
            wxUserInfo.HeadImgUrl = userInfo.headimgurl;
            wxUserInfo.NickName = userInfo.nickname;
            wxUserInfo.OpenId = userInfo.openid;
            wxUserInfo.Province = userInfo.province;
            wxUserInfo.Sex = userInfo.sex;
            wxUserInfo.UnionId = userInfo.unionid;
        }

        public string GetToken()
        {
            try
            {
                var config = this.WorkContext.WeiXinMPConfig;
                string appId = config.WeixinAppId;
                string appSecret = config.WeixinAppSecret;
                return GetToken(appId, appSecret);
                //var access_token = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(appId, appSecret);
                //return access_token.access_token;// result.access_token;// Json(result, JsonRequestBehavior.AllowGet);
                //if (!AccessTokenContainer.CheckRegistered(appId))
                //{
                //    AccessTokenContainer.Register(appId, appSecret);
                //}
                //var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(appId, appSecret);//AccessTokenContainer.GetTokenResult(appId);                

                //也可以直接一步到位：
                //return AccessTokenContainer.TryGetAccessToken(appId, appSecret);
            }
            catch (Exception ex)
            {
                //TODO:为简化代码，这里不处理异常（如Token过期）
                throw ex;// Json(new { error = "执行过程发生错误！" }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetToken(string appId, string appSecret)
        {
            return AccessTokenContainer.TryGetAccessToken(appId, appSecret);
        }

        public OAuthAccessTokenResult GetOAuthAccessTokenResult(string code)
        {
            var config = this.WorkContext.WeiXinMPConfig;
            string appId = config.WeixinAppId;
            string appSecret = config.WeixinAppSecret;
            return GetOAuthAccessTokenResult(appId, appSecret, code);
        }

        public OAuthAccessTokenResult GetOAuthAccessTokenResult(string appId, string appSecret, string code)
        {
            if (!OAuthAccessTokenContainer.CheckRegistered(appId))
            {
                OAuthAccessTokenContainer.Register(appId, appSecret);
            }
            return OAuthAccessTokenContainer.GetOAuthAccessTokenResult(appId, code, false);
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
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
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
