using SharedUtilitys.ConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pasys.Web.Admin.UI.AppConfig
{
    public class LocalConfigInfo:IConfigInfo
    {        
        public string Ver { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }

    [System.Xml.Serialization.XmlType("LocalConfigInfo")]
    public class LocalConfig : IConfigInfo
    {
        public string Ver { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
