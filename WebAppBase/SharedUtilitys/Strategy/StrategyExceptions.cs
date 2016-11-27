using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core
{
    public class StrategyCreateFailedException : Exception
    {
        public StrategyCreateFailedException() : base() { }
        public StrategyCreateFailedException(string message) : base(string.Format("Load Stratety Failed:{0}", message)) { }
        public StrategyCreateFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
