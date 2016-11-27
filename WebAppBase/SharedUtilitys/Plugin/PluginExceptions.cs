using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core
{
    public class PluginCreateFailedException : Exception
    {
        public PluginCreateFailedException() : base() { }
        public PluginCreateFailedException(string message) : base(string.Format("Load Plugin Failed:{0}", message)) { }
        public PluginCreateFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
