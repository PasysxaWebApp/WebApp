﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Pasys.Web.Admin.UI.Configs;
using Pasys.Web.Admin.UI.Enums;
using System.Text;
using Pasys.Web.Identity.Models;

namespace Pasys.Web.Admin.UI.Models.SystemMenus
{
    public class SystemMenuListModel
    {
        public static SystemMenuListModel GetInstance()
        {
            return new SystemMenuListModel();
        }

        private SystemMenuListModel()
        {
        }

        private StringBuilder _contentText = new StringBuilder();// String.Empty;
        private UrlHelper _helper;

        public string GetResponsiveHtml(List<ApplicationFunction> menus, UrlHelper helper)
        {
            _contentText = new StringBuilder();// String.Empty;
            _helper = helper;

            //_createMenuList(SystemMenuConfig.GetMenus(rollEnum), 0, true);

            //_contentText.AppendLine("                <ul class=\"nav navbar-nav\">");
            //_createMenuList_ForBootstrap(SystemMenuConfig.GetMenus(rollEnum), 0, true);
            //_contentText.AppendLine("                </ul>");

            _contentText.AppendLine("                <ul>");
            _createMenuList_ForResponsivemenu(menus, 0, true);
            _contentText.AppendLine("                </ul>");

            return _contentText.ToString();
        }

        public string GetBootStrapHtml(List<ApplicationFunction> menus, UrlHelper helper)
        {
            _contentText = new StringBuilder();// String.Empty;
            _helper = helper;

            //_createMenuList(SystemMenuConfig.GetMenus(rollEnum), 0, true);

            _contentText.AppendLine("                <ul class=\"nav navbar-nav\">");
            _createMenuList_ForBootstrap(menus, 0, true);
            _contentText.AppendLine("                </ul>");

            return _contentText.ToString();
        }


        private void _createMenuList(List<ApplicationFunction> menus, int target, bool isRoot)
        {
            var systemMenuModels = menus.FindAll(model=>
            {
                if ((isRoot && model.FunctionId == model.ParentFunctionId)
                    || (!isRoot && model.FunctionId != model.ParentFunctionId && target == model.ParentFunctionId))
                {
                    return true;
                }
                return false;
            });

            if (systemMenuModels.Count > 0)
            {
                _contentText.AppendLine("        <ul id=\"flyoutmenu\">  " + Environment.NewLine);
            }

            var cnt = 0;

            foreach (var item in systemMenuModels)
            {
                cnt++;

                string url;

                if (string.IsNullOrEmpty(item.ActionName) || string.IsNullOrEmpty(item.ControllerName) || item.ActionName.Length == 0 || item.ControllerName.Length == 0)
                {
                    url = "#";
                }
                else
                {
                    url = _helper.Action(item.ActionName, item.ControllerName);
                }

                var last = string.Empty;

                if (cnt >= systemMenuModels.Count)
                {
                    last = " class=\"last\"";
                }

                if (item.MenuName.StartsWith("--") && url.Equals("#"))
                {
                    _contentText.AppendLine("<li class='title' style='border-radius: 0px;margin: 0px;background:none;border-top:#dfdfdf 1px dashed;padding-bottom: 0px;'>");
                }
                else
                {
                    _contentText.AppendLine(String.Format("            <li{2}><a href=\"{0}\" >{1}</a> " + Environment.NewLine, url, item.MenuName, last));
                }
                _createMenuList(menus, item.FunctionId, false);
                _contentText.AppendLine(string.Format("            </li> " + Environment.NewLine, url));
            }

            if (systemMenuModels.Count > 0)
            {
                _contentText.AppendLine("        </ul>  " + Environment.NewLine);
            }
        }

        private void _createMenuList_ForBootstrap(List<ApplicationFunction> menus, int target, bool isRoot)
        {
            var systemMenuModels = menus.FindAll(model=>
            {
                if ((isRoot && model.FunctionId == model.ParentFunctionId)
                    || (!isRoot && model.FunctionId != model.ParentFunctionId && target == model.ParentFunctionId))
                {
                    return true;
                }
                return false;
            });


            //var cnt = 0;

            foreach (var item in systemMenuModels)
            {
                //cnt++;
                string url;

                if (string.IsNullOrEmpty(item.ActionName) || string.IsNullOrEmpty(item.ControllerName) || item.ActionName.Length == 0 || item.ControllerName.Length == 0)
                {
                    url = "#";
                }
                else
                {
                    url = _helper.Action(item.ActionName, item.ControllerName);
                }

                bool hasChildItem = menus.FindAll(m => m.ParentFunctionId == item.FunctionId && m.ParentFunctionId != m.FunctionId).Count > 0;

                if (item.IsRootMenu)
                {
                    if (hasChildItem)
                    {
                        _contentText.AppendLine("        <li class='dropdown'>  ");
                        _contentText.AppendLine("                        <a  href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-expanded=\"false\" style=\"color: White;\">");
                        _contentText.AppendLine("                            <div style=\"text-align:center;\">");
                        _contentText.AppendFormat("                                <em class=\"icons_24 {0}\"></em>", item.CssClass);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                            </div>");
                        _contentText.AppendLine("                            <div style=\"text-align:center;\">");
                        _contentText.AppendFormat("                                {0} <span class=\"caret\" style=\"color: White;\">", item.MenuName);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                            </div>");
                        _contentText.AppendLine("                        </a>");
                        _contentText.AppendLine("                        <ul class=\"dropdown-menu\" role=\"menu\">");
                        _createMenuList_ForBootstrap(menus, item.FunctionId, false);
                        _contentText.AppendLine("                        </ul>");
                        _contentText.AppendLine("        </li>  ");
                    }
                    else
                    {
                        _contentText.AppendLine("        <li>  ");
                        _contentText.AppendFormat("                        <a href=\"{0}\" style=\"color: White;\">", url);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                            <div style=\"text-align:center;\">");
                        _contentText.AppendFormat("                                <em class=\"icons_24 {0}\"></em>", item.CssClass);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                            </div>");
                        _contentText.AppendLine("                            <div style=\"text-align:center;\">");
                        _contentText.AppendFormat("                                {0}", item.MenuName);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                            </div>");
                        _contentText.AppendLine("                        </a>");
                        _contentText.AppendLine("        </li>  ");
                    }
                }
                else
                {
                    if (item.SeparateMenuFlag)
                    {
                        _contentText.AppendLine("                   <li class=\"divider\"></li>");
                    }

                    _contentText.AppendFormat("       <li><a href=\"{0}\">{1}</a></li>", url, item.MenuName);
                    _contentText.AppendLine("");
                }

            }
        }

        private void _createMenuList_ForResponsivemenu(List<ApplicationFunction> menus, int target, bool isRoot)
        {
            var systemMenuModels = menus.FindAll(model =>
            {
                if ((isRoot && model.FunctionId == model.ParentFunctionId)
                    || (!isRoot && model.FunctionId != model.ParentFunctionId && target == model.ParentFunctionId))
                {
                    return true;
                }
                return false;
            });


            //var cnt = 0;

            foreach (var item in systemMenuModels)
            {
                if (!item.ShowInMenu)
                {
                    continue;
                }
                //cnt++;
                string url;

                if (string.IsNullOrEmpty(item.ActionName) || string.IsNullOrEmpty(item.ControllerName) || item.ActionName.Length == 0 || item.ControllerName.Length == 0)
                {
                    url = "#";
                }
                else
                {
                    url = _helper.Action(item.ActionName, item.ControllerName);
                }

                bool hasChildItem = menus.FindAll(m => m.ParentFunctionId == item.FunctionId && m.ParentFunctionId != m.FunctionId).Count > 0;

                if (item.IsRootMenu)
                {
                    if (hasChildItem)
                    {
                        _contentText.AppendLine("                        <li>");
                        _contentText.AppendLine("                            <a href=\"#\">");
                        _contentText.AppendLine("                                <div style=\"text-align: center;height:35px;\">");
                        _contentText.AppendFormat("                                    <p  class=\"icons_24 {0}\" />", item.CssClass);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                                </div>");
                        _contentText.AppendFormat("                                <p style=\"margin: 0px;\">{0}</p>", item.MenuName);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                           </a>");
                        _contentText.AppendLine("                        <ul class=\"dropdown-menu\" role=\"menu\">");
                        _createMenuList_ForResponsivemenu(menus, item.FunctionId, false);
                        _contentText.AppendLine("                        </ul>");
                        _contentText.AppendLine("        </li>  ");

                    }
                    else
                    {
                        if (item.SeparateMenuFlag)
                        {
                            _contentText.AppendLine("                   <li class=\"divider\"></li>");
                        }
                        _contentText.AppendLine("                        <li>");
                        _contentText.AppendFormat("                            <a href=\"{0}\">", url);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                                <div style=\"text-align: center;height:35px;\">");
                        _contentText.AppendFormat("                                    <p  class=\"icons_24 {0}\" />", item.CssClass);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                                </div>");
                        _contentText.AppendFormat("                                <p style=\"margin: 0px;\">{0}</p>", item.MenuName);
                        _contentText.AppendLine("");
                        _contentText.AppendLine("                           </a>");
                        _contentText.AppendLine("        </li>  ");
                    }
                }
                else
                {
                    if (item.SeparateMenuFlag)
                    {
                        _contentText.AppendLine("                   <li class=\"divider\"></li>");
                    }
                    _contentText.AppendFormat("       <li><a href=\"{0}\">{1}</a></li>", url, item.MenuName);
                    _contentText.AppendLine("");
                }
            }
        }

    }
}