using Pasys.Core.IOC;

namespace Pasys.Core.Models
{
    public class AutoComplete : IEntity
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
