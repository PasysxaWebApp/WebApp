using System.Data.Common;
using System.Linq;
using Pasys.Core.MetaData;
using Pasys.Core.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Pasys.Core.Extend;
using Pasys.Core.IOC;
using Pasys.Core.Logs;

namespace Pasys.Core.Data.DataBase
{
    /// <summary>
    /// 数据库基类，AppConfig> DataBase>[SQL,Jet,Ace]
    /// </summary>
    public abstract class DataBasic : IDependency
    {


    }
}
