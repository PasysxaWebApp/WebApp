using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core.Strategy
{
    public interface IStrategy
    {
    }

    public class StrategyInfo
    {
        private string _filename;
        private string _classname;
        private string _assemblyname;

        public string StrategyName { get; set; }
        public string NiceName { get; set; }

        public string FileName {
            get
            {
                if (!string.IsNullOrEmpty(_filename)) {
                    return _filename;
                }
                return string.Format("{0}.dll", AssemblyName);
            }
            set {
                _filename = value;
            }
        }
        public string ClassName
        {
            get
            {
                if (!string.IsNullOrEmpty(_classname))
                {
                    return _classname;
                }
                var names = AssemblyName.Split(new char[] { '.' });
                if (names.Length > 2)
                {
                    return string.Format("{0}.{1}{2}", AssemblyName, names[names.Length - 1], names[names.Length - 2]);
                }
                else {
                    return string.Format("{0}.{1}", AssemblyName, "Strategy");
                }
            }
            set
            {
                _classname = value;
            }
        }
        public string AssemblyName
        {
            get
            {
                if (!string.IsNullOrEmpty(_assemblyname))
                {
                    return _assemblyname;
                }
                return string.Format("Pasys.Strategy.{0}", StrategyName);
            }
            set
            {
                _assemblyname = value;
            }
        }
    }

}
