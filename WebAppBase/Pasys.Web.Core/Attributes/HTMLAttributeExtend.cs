using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.Attributes
{
    public static class HTMLAttributeExtend
    {
        public static IViewModelAttribute GetEditViewModelAttribute(this Type t)
        {
            var vmAttr = t.GetCustomAttributes(typeof(EditViewModelAttribute), true).FirstOrDefault() as IViewModelAttribute;
            return vmAttr;
        }
        public static IViewModelAttribute GetListViewModelAttribute(this Type t)
        {
            var vmAttr = t.GetCustomAttributes(typeof(ListViewModelAttribute), true).FirstOrDefault() as IViewModelAttribute;
            return vmAttr;
        }

    }
}
