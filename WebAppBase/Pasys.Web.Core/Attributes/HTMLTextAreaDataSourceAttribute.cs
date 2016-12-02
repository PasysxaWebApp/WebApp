using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.Attributes
{
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

        public TextAreaSourceAttributeBase CreateDefault()
        {
            return new DefaultTextAreaDataSourceAttribute();
        }
    }

    public class DefaultTextAreaDataSourceAttribute : TextAreaSourceAttributeBase
    {
    }


    public class TextAreaDataSourceAttribute : TextAreaSourceAttributeBase
    {
    }

}
