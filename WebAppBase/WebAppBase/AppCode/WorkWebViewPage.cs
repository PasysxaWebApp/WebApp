using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppBase
{
    /// <summary>
    /// 视图页面基类
    /// </summary>
    public abstract class WorkWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public WorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);//启用客户端验证
            Html.EnableUnobtrusiveJavaScript(true);//启用非侵入式脚本
            var workController = this.ViewContext.Controller as WorkController;
            if (workController != null)
            {
                WorkContext = (WorkContext)workController.WorkContext;
            }
            else {
                WorkContext = new WorkContext();
            }
        }

        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }

    /// <summary>
    /// 视图页面基类
    /// </summary>
    public abstract class WorkWebViewPage : WorkWebViewPage<dynamic>
    {

    }

}
