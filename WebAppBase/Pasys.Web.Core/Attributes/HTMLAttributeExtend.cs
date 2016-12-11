using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.Attributes
{
    public static class HTMLAttributeExtend
    {
        public static IViewModelAttribute GetViewModelAttribute(this Type t)
        {
            var vmAttr = t.GetCustomAttributes(typeof(IViewModelAttribute), true).FirstOrDefault() as IViewModelAttribute;
            return vmAttr;
        }

    }
}
