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
                _rdbsconfiginfo = ConfigInfoOperater.LoadConfigInfo<RDBSConfigInfo>();
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
                _globalconfiginfo = ConfigInfoOperater.LoadConfigInfo<GlobalConfigInfo>();
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
                _smsconfiginfo = ConfigInfoOperater.LoadConfigInfo<SMSConfigInfo>();
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
                _weixinmpconfiginfo = ConfigInfoOperater.LoadConfigInfo<WeiXinMPConfigInfo>();
            }
            return _weixinmpconfiginfo;
        }

    }
}
