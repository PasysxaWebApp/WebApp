using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.Attributes
{
    public interface IViewModelAttribute
    {
        string Title { get; set; }
        string SubTitle { get; set; }
    }
    public class ViewModelAttributeBase : Attribute, IViewModelAttribute
    {
        private string _title;
        private string _subTitle;
        public virtual Type ResourceType { get; set; }

        public virtual string Title
        {
            get
            {
                return GetPropertyValue(_title);
            }
            set
            {
                _title = value;
            }
        }
        public virtual string SubTitle
        {
            get
            {
                return GetPropertyValue(_subTitle);
            }
            set
            {
                _subTitle = value;
            }
        }


        protected string GetPropertyValue(string resourceKey)
        {
            if (ResourceType == null)
            {
                return resourceKey;
            }

            var resourceValue = string.Format("{0}", System.Web.HttpContext.GetGlobalResourceObject(ResourceType.Name, resourceKey));
            return resourceValue;
        }

    }

    public class ViewModelAttribute : ViewModelAttributeBase
    { 
    
    }
}
