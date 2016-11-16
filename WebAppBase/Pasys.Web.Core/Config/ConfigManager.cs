using SharedUtilitys.ConfigInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core
{
    public class ConfigManager
    {
        private static GlobalConfigInfo _globalconfiginfo = null;//全局基本配置信息
        private static RDBSConfigInfo _rdbsconfiginfo = null;//关系数据库配置信息
        private static SMSConfigInfo _smsconfiginfo = null;//短信配置信息
        private static WeiXinMPConfigInfo _weixinmpconfiginfo = null;//微信配置信息

        static ConfigManager()
        {
        }

        /// <summary>
        /// 获取关系数据库配置
        /// </summary>
        public static RDBSConfigInfo GetRDBSConfig()
        {
            if (_rdbsconfiginfo == null)
            {
                _rdbsconfiginfo = ConfigInfoOperator.LoadConfigInfo<RDBSConfigInfo>();
            }
            return _rdbsconfiginfo;
        }


        /// <summary>
        /// 获取全局基本配置信息
        /// </summary>
        public static GlobalConfigInfo GetGlobalConfig()
        {
            if (_globalconfiginfo == null)
            {
                _globalconfiginfo = ConfigInfoOperator.LoadConfigInfo<GlobalConfigInfo>();
            }
            return _globalconfiginfo;
        }

        /// <summary>
        /// 获取短信配置信息
        /// </summary>
        public static SMSConfigInfo GetSMSConfigInfo()
        {
            if (_smsconfiginfo == null)
            {
                _smsconfiginfo = ConfigInfoOperator.LoadConfigInfo<SMSConfigInfo>();
            }
            return _smsconfiginfo;
        }

        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        /// <returns></returns>
        public static WeiXinMPConfigInfo GetWeiXinMPConfigInfo()
        {
            if (_weixinmpconfiginfo == null)
            {
                _weixinmpconfiginfo = ConfigInfoOperator.LoadConfigInfo<WeiXinMPConfigInfo>();
            }
            return _weixinmpconfiginfo;
        }

        public static T GetConfig<T>() where T : IConfigInfo
        {
                return ConfigInfoOperator.LoadConfigInfo<T>();
        }

        public static T GetConfig<T>(string configFileName) where T : IConfigInfo
        {
            var fileName = string.Format("/App_Data/{0}.config", configFileName);
            return ConfigInfoOperator.LoadConfigInfo<T>(fileName);
        }
    }
}
