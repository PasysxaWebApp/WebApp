using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.Attributes
{

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
}
