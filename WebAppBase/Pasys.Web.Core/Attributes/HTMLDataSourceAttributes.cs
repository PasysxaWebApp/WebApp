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
        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }
        public virtual SelectList GetData(object selectedValue)
        {
            throw new NotImplementedException();
        }        
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

    public abstract class TextBoxSourceAttributeBase : Attribute, ITextBoxSourceAttribute
    {
        public virtual string TemplateName
        {
            get
            {
                return "TextBoxes";
            }
        }
        public virtual string PlaceHolder { get; set; }

        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }
        public virtual string GetData(object textValue)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class TextAreaSourceAttributeBase : Attribute, ITextAreaSourceAttribute
    {
        public virtual string TemplateName
        {
            get
            {
                return "TextAreas";
            }
        }
        public virtual Dictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
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




    public class TextBoxDataSourceAttribute : TextBoxSourceAttributeBase
    {
        private string _placeHolder;
        public override string PlaceHolder
        {
            get
            {
                if (ResourceType == null)
                {
                    return _placeHolder;
                }

                var p = string.Format("{0}", System.Web.HttpContext.GetGlobalResourceObject(ResourceType.Name, _placeHolder));
                return p;
            }
            set
            {
                _placeHolder = value;
            }
        }
        public Type ResourceType { get; set; }
        public TextBoxDataSourceAttribute()
        {

        }

        public override string GetData(object textValue)
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


    public class TextAreaDataSourceAttribute : TextAreaSourceAttributeBase
    {
    }
}
