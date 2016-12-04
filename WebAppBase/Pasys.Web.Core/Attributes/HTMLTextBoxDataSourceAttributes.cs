using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pasys.Web.Core.Attributes
{
    public class YesNoDataSourceAttribute : RadioButtonDataSourceAttributeBase
    {
        private string[] yesStrs = { "yes", "true", "1" };
        private string _yesName, _noName;
        public string YesName
        {
            get
            {
                return GetPropertyValue(_yesName);
            }
            set
            {
                _yesName = value;
            }

        }
        public string NoName
        {
            get
            {
                return GetPropertyValue(_noName);
            }
            set
            {
                _noName = value;
            }
        }
        public YesNoDataSourceAttribute()
        {
            YesName = "Yes";
            NoName = "No";
            this.ListItems.Add(new SelectListItem() { Value = "1", Text = "Yes" });
            this.ListItems.Add(new SelectListItem() { Value = "0", Text = "No" });
        }

        public override SelectList GetData(object selectedValue)
        {
            string s = string.Format("{0}", selectedValue).ToLower();
            var sv = "0";
            if (yesStrs.Contains(s))
            {
                sv = "1";
            }
            var lst = new SelectList(ListItems, "Value", "Text", sv);
            return lst;
        }
    }

}
