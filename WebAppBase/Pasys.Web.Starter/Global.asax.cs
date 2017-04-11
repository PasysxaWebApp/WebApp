using Pasys.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Pasys.Core.Extend;

namespace Pasys.Web.Starter
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoadModules();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void LoadModules()
        {
            Type plugBaseType = typeof(PluginBase);

            var types = BuildManager.GetReferencedAssemblies().Cast<Assembly>().SelectMany(assembly => assembly.GetTypes()).ToArray();
            types.Each(p =>
            {
                if (plugBaseType.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface)
                {
                    var plug = Activator.CreateInstance(p) as PluginBase;
                    if (plug != null)
                    {
                        //var moduleRoutes = plug.RegistRoute();
                        //if (moduleRoutes != null)
                        //{
                        //    var routeArray = moduleRoutes.ToArray();
                        //    if (routeArray.Length > 0)
                        //    {
                        //        routes.AddRange(routeArray);
                        //    }
                        //}
                        //plug.Excute();
                    }
                }
            });

        }
    }
}
