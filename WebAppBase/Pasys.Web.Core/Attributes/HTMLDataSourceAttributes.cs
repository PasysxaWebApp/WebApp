using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pasys.Web.Core.Attributes
{

    public interface IDataSourceAttribute
    {
        string TemplateName { get; }

        Dictionary<string, object> GetAttributes();
        void AddAttribute(string attributeName, string attributeValue);
        void RemoveAttribute(string attributeName);

        List<string> GetClass();
        void AddClass(string name);
        void RemoveClass(string name);


        Dictionary<string, object> GetStyles();
        void AddStyle(string properyt, string value);
        void RemoveStyle(string properyt);
    }

    public abstract class DataSourceAttribute : Attribute, IDataSourceAttribute
    {
        public virtual string TemplateName { get; set; }
        public virtual Type ResourceType { get; set; }

        private Dictionary<string, object> _attributes = new Dictionary<string, object>();
        private List<string> _classes = new List<string>();
        private Dictionary<string, object> _styles = new Dictionary<string, object>();

        public Dictionary<string, object> GetAttributes()
        {
            return _attributes;
        }
        public void AddAttribute(string attributeName, string attributeValue)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                return;
            }
            attributeName = attributeName.ToLower();
            if (!_attributes.ContainsKey(attributeName))
            {
                _attributes.Add(attributeName, attributeValue);
            }
            else
            {
                _attributes[attributeName] = attributeValue;
            }
        }

        public void RemoveAttribute(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                return;
            }
            attributeName = attributeName.ToLower();
            if (_attributes.ContainsKey(attributeName))
            {
                _attributes.Remove(attributeName);
            }
        }

        public List<string> GetClass()
        {
            return _classes;
        }
        public void AddClass(string name)
        { 
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            name = name.ToLower();
            if (!_classes.Contains(name))
            {
                _classes.Add(name);
            }
        }
        public void RemoveClass(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            name = name.ToLower();
            if (_classes.Contains(name))
            {
                _classes.Remove(name);
            }
        }


        public Dictionary<string, object> GetStyles()
        {
            return _styles;
        }
        public void AddStyle(string styleName, string styleValue)
        {
            if (string.IsNullOrEmpty(styleName))
            {
                return;
            }
            styleName = styleName.ToLower();
            if (!_styles.ContainsKey(styleName))
            {
                _styles.Add(styleName, styleValue);
            }
            else
            {
                _styles[styleName] = styleValue;
            }
        }
        public void RemoveStyle(string styleName)
        {
            if (string.IsNullOrEmpty(styleName))
            {
                return;
            }
            styleName = styleName.ToLower();
            if (_styles.ContainsKey(styleName))
            {
                _styles.Remove(styleName);
            }
        }

        protected string GetPropertyValue( string resourceKey)
        {
            if (ResourceType == null)
            {
                return resourceKey;
            }

            var resourceValue = string.Format("{0}", System.Web.HttpContext.GetGlobalResourceObject(ResourceType.Name, resourceKey));
            return resourceValue;
        }
    }

    public abstract class DropDownDataSourceAttributeBase : DataSourceAttribute
    {
        public const string DefaultTemplateName = "DropdownList";
        //DropdownList
        public List<SelectListItem> ListItems { get; set; }
        public DropDownDataSourceAttributeBase()
        {
            TemplateName = DefaultTemplateName;
            ListItems = new List<SelectListItem>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            return new SelectList(ListItems, "Value", "Text", selectedValue);
        }
    }

    public abstract class RadioButtonDataSourceAttributeBase : DataSourceAttribute
    {
        public const string DefaultTemplateName = "RadioButtons";
        public List<SelectListItem> ListItems { get; set; }
        public RadioButtonDataSourceAttributeBase()
        {
            TemplateName = DefaultTemplateName;
            ListItems = new List<SelectListItem>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            return new SelectList(ListItems, "Value", "Text", selectedValue);
        }
    }

    public abstract class CheckBoxDataSourceAttributeBase : DataSourceAttribute
    {
        public const string DefaultTemplateName = "CheckBoxes";
        public List<SelectListItem> ListItems { get; set; }

        public CheckBoxDataSourceAttributeBase()
        {
            TemplateName = DefaultTemplateName;
            ListItems = new List<SelectListItem>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            var svs = selectedValue as List<string>;
            if (svs == null)
            {
                svs = new List<string>();
            }
            if (ListItems != null)
            {
                foreach (var item in ListItems)
                {
                    item.Selected = svs.Contains(item.Value);
                }
            }
            return new SelectList(ListItems, "Value", "Text");
        }
    }

    public abstract class TextBoxDataSourceAttributeBase : DataSourceAttribute
    {
        private string _placeHolder;
        public const string DefaultTemplateName = "TextBoxes";
        public TextBoxDataSourceAttributeBase()
        {
            TemplateName = DefaultTemplateName;
            PlaceHolder = "";
        }
        public virtual string PlaceHolder
        {
            get
            {
                return GetPropertyValue(_placeHolder);
            }
            set
            {
                _placeHolder = value;
            }
        }
        public virtual string GetData(object textValue)
        {
            if (textValue != null)
            {
                return textValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public abstract class TextAreaDataSourceAttributeBase : DataSourceAttribute
    {
        public const string DefaultTemplateName = "TextAreas";
        public TextAreaDataSourceAttributeBase()
        {
            TemplateName = DefaultTemplateName;
        }
        public virtual string GetData(object textValue)
        {
            if (textValue != null)
            {
                return textValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

}
