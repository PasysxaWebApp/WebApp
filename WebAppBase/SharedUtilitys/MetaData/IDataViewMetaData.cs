using Pasys.Core.ViewPort;
using Pasys.Core.ViewPort.Descriptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pasys.Core.MetaData
{
    public interface IDataViewMetaData
    {
        Dictionary<string, BaseDescriptor> ViewPortDescriptors { get; }
        Dictionary<string, PropertyInfo> Properties { get; }
        Type TargetType { get; }
    }
}
