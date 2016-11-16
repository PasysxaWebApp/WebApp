using SharedUtilitys.ConfigInfo;
using System;
using System.Collections.Generic;

namespace Pasys.Web.Core
{
    /// <summary>
    /// 关系型数据库配置信息
    /// </summary>
    [Serializable]
    public class RDBSConfigInfo : IConfigInfo
    {
        public string Ver { get; set; }
        
        //关系数据库连接字符串
        public List<ConnectionString> ConnectionStrings { get; set; }

        /// <summary>
        /// 关系数据库连接字符串
        /// </summary>
        public string DefaultConnectString
        {
            get
            {
                if (ConnectionStrings.Count > 0)
                {
                    return ConnectionStrings[0].ConnectionStr;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 关系数据库类型
        /// </summary>
        public string DatabaseType { get; set; }

    }

    public class ConnectionString
    {
        public string Name { get; set; }
        public string ConnectionStr { get; set; }
        public string ProviderName { get; set; }
    }
}
