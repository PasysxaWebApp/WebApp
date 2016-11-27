using System;
using Pasys.Core.Enums;

namespace Pasys.Core.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class RegistryLocationAttribute : Attribute
    {
        public RegistryLocationEnum RegistryLocation { get; set; }
        public string ParentKey { get; set; }
    }
}
