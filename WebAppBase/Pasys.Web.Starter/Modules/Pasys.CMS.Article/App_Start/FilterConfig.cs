using System.Web;
using System.Web.Mvc;

namespace Pasys.CMS.Article
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
