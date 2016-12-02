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

    public interface IDropDownDataSourceAttribute : IDataSourceAttribute
    {
        SelectList GetData(object selectedValue);
    }

    public interface IRadioButtonDataSourceAttribute : IDataSourceAttribute
    {
        SelectList GetData(object selectedValue);
    }

    public interface ICheckBoxDataSourceAttribute : IDataSourceAttribute
    {
        SelectList GetData(object selectedValue);
    }

    public interface ITextBoxSourceAttribute : IDataSourceAttribute
    {
        string GetData(object textValue);
        string PlaceHolder { get; }
    }

    public interface ITextAreaSourceAttribute : IDataSourceAttribute
    {
        string GetData(object textValue);
    }

    public abstract class DropDownDataSourceAttributeBase : Attribute, IDropDownDataSourceAttribute
    {
        public virtual string TemplateName
        {
            get
            {
                return "DropdownList";
            }
        }
        public virtual SelectList GetData(object selectedValue)
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }
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

    public abstract class RadioButtonDataSourceAttributeBase : Attribute, IRadioButtonDataSourceAttribute
    {
        public virtual string TemplateName
        {
            get
            {
                return "RadioButtons";
            }
        }
        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CheckBoxDataSourceAttributeBase : Attribute, ICheckBoxDataSourceAttribute
    {
        public virtual string TemplateName
        {
            get
            {
                return "CheckBoxes";
            }
        }
        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            throw new NotImplementedException();
        }
    }
}
