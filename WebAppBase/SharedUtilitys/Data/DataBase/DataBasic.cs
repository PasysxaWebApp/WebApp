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
        public const string DataBaseAppSetingKey = "DataBase";
        public const string ConnectionKey = "Easy";

        public abstract IEnumerable<string> DataBaseTypeNames();

        public virtual T Get<T>(params object[] primaryKeys) where T : class
        {
            return default(T);
        }

    }
}
