using System.Web.Mvc;

namespace Pasys.Web.Areas.Test.Areas.abc
{
    public class abcAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "abc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "abc_default",
                "abc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}