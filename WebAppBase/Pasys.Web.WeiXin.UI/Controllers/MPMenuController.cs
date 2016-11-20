using Pasys.Web.WeiXin.UI.Models;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pasys.Web.WeiXin.UI.Controllers
{
    public class MPMenuController : WorkController
    {
        // GET: MPMenu
        public ActionResult Index()
        {
            var model = new MPMenuViewModel()
            {
                ResultFull = new GetMenuResultFull()
            };
            model.ResultFull.menu = new MenuFull_ButtonGroup();
            model.ResultFull.menu.button = new List<MenuFull_RootButton>();

            var list = GetWeiXinMPMenus();
            var rootButtons = list.FindAll(m => m.IsRoot == true);
            for (var i = 0; i < 3; i++)
            {
                MPMenuInfo rootButton = null;
                if (rootButtons.Count > i)
                {
                    rootButton = rootButtons[i];
                }
                else
                {
                    rootButton = new MPMenuInfo()
                    {
                        Col = i,
                        IsRoot = true,
                        Type = "click"
                    };
                }
                var rb = new MenuFull_RootButton()
                {
                    name = rootButton.Name,
                    key = rootButton.MenuKey,
                    type = rootButton.Type,
                    url = rootButton.MenuUrl,
                    sub_button = new List<MenuFull_RootButton>()
                };
                var buttons = list.FindAll(m => m.Col == rootButton.Col && m.IsRoot == false);
                for (int j = 0; j < 5; j++)
                {
                    MPMenuInfo button = null;
                    if (buttons.Count > j)
                    {
                        button = buttons[j];
                    }
                    else
                    {
                        button = new MPMenuInfo()
                        {
                            Col = i,
                            Row = j,
                            IsRoot = false,
                            Type = "click"
                        };
                    }
                    var rsb = new MenuFull_RootButton()
                    {
                        name = button.Name,
                        key = button.MenuKey,
                        type = button.Type,
                        url = button.MenuUrl,
                    };
                    rb.sub_button.Add(rsb);
                }
                model.ResultFull.menu.button.Add(rb);

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveMenu(MPMenuViewModel request)
        {
            var list = new List<MPMenuInfo>();
            for (var j = 0; j < request.ResultFull.menu.button.Count; j++)
            {
                var button = request.ResultFull.menu.button[j];
                var menuInfoRoot = new MPMenuInfo()
                {
                    Name = button.name,
                    MenuKey = button.key,
                    Type = button.type,
                    MenuUrl = button.url,
                    IsRoot = true,
                    Row = 0,
                    Col = j,
                    CreateID = this.WorkContext.UserId,
                    UpdateID = this.WorkContext.UserId,
                    OpenId = ""
                };

                list.Add(menuInfoRoot);

                for (int i = 0; i < button.sub_button.Count; i++)
                {
                    var subButton = button.sub_button[i];
                    var menuInfo = new MPMenuInfo()
                    {
                        Name = subButton.name,
                        MenuKey = subButton.key,
                        Type = subButton.type,
                        MenuUrl = subButton.url,
                        IsRoot = false,
                        Row = i,
                        Col = j,
                        CreateID = this.WorkContext.UserId,
                        UpdateID = this.WorkContext.UserId,
                        OpenId = ""
                    };

                    list.Add(menuInfo);
                }

            }

            var bl = InsertWeiXinMenus(list);
            var json = new
            {
                Success = bl,
                Message = bl ? "菜单保存成功。" : "菜单保存失败。"
            };
            return Json(json);
        }

        [HttpPost]
        public ActionResult CreateMenu(MPMenuViewModel request)
        {
            string token = GetToken(); // request.token;
            GetMenuResultFull resultFull = request.ResultFull;
            MenuMatchRule menuMatchRule = request.MenuMatchRule;

            var useAddCondidionalApi = menuMatchRule != null && !menuMatchRule.CheckAllNull();
            var apiName = string.Format("使用接口：{0}。", (useAddCondidionalApi ? "个性化菜单接口" : "普通自定义菜单接口"));
            try
            {
                //重新整理按钮信息
                WxJsonResult result = null;
                IButtonGroupBase buttonGroup = null;
                if (useAddCondidionalApi)
                {
                    //个性化接口
                    buttonGroup = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenuFromJsonResult(resultFull, new ConditionalButtonGroup()).menu;

                    var addConditionalButtonGroup = buttonGroup as ConditionalButtonGroup;
                    addConditionalButtonGroup.matchrule = menuMatchRule;
                    result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenuConditional(token, addConditionalButtonGroup);
                    apiName += string.Format("menuid：{0}。", (result as CreateMenuConditionalResult).menuid);
                }
                else
                {
                    //普通接口
                    buttonGroup = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                    result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(token, buttonGroup);
                }

                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = "菜单更新成功。" + apiName
                };
                return Json(json);
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = string.Format("更新失败：{0}。{1}", ex.Message, apiName) };
                return Json(json);
            }
        }

        [HttpPost]
        public ActionResult GetMenu()
        {
            string token = GetToken();
            var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenu(token);
            if (result == null)
            {
                return Json(new { error = "菜单不存在或验证失败！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public ActionResult DeleteMenu()
        {
            try
            {
                string token = GetToken();
                var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.DeleteMenu(token);
                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = result.errmsg
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = ex.Message };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        private List<MPMenuInfo> GetWeiXinMPMenus()
        {
            throw new NotImplementedException();
        }

        private bool InsertWeiXinMenus(List<MPMenuInfo> menus)
        {
            throw new NotImplementedException();
        }
    }
}