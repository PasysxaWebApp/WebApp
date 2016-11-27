using Pasys.Core.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core.ConfigInfo
{
    public interface IConfigInfo
    {
        string Ver { get; set; }
    }

    public class ConfigInfoOperator
    {
        public static T LoadConfigInfo<T>() where T : IConfigInfo
        {
            var tp = typeof(T);
            var mapPath = string.Empty;
            string configInfoFile = tp.Name;
            var fileName = string.Format("~/App_Data/{0}.config", configInfoFile);
            if (IOHelper.MapPathExists(fileName))
            {
                mapPath = IOHelper.GetMapPath(fileName);
            }
            else if (configInfoFile.ToLower().EndsWith("configinfo"))
            {
                fileName = string.Format("~/App_Data/{0}.config", configInfoFile.Substring(0, configInfoFile.Length - 10));
                if (IOHelper.MapPathExists(fileName))
                {
                    mapPath = IOHelper.GetMapPath(fileName);
                }
            }
            else if (configInfoFile.ToLower().EndsWith("config"))
            {
                fileName = string.Format("~/App_Data/{0}.config", configInfoFile.Substring(0, configInfoFile.Length - 6));
                if (IOHelper.MapPathExists(fileName))
                {
                    mapPath = IOHelper.GetMapPath(fileName);
                }
            }
            return LoadConfigInfo<T>(mapPath);
        }
        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="configInfoType">配置信息类型</param>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        public static T LoadConfigInfo<T>(string configInfoFile) where T : IConfigInfo
        {
            if (File.Exists(configInfoFile))
            {
                return (T)IOHelper.DeserializeFromXML(typeof(T), configInfoFile);
            }
            else
            {
                throw new FileNotFoundException();

            }
        }

        /// <summary>
        /// 将配置信息保存到文件中
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="configInfoFile">保存路径</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveConfigInfo<T>(T configInfo, string configInfoFile) where T : IConfigInfo
        {
            return IOHelper.SerializeToXml(configInfo, configInfoFile);
        }

    }

    /// <summary>
    /// 基于文件的配置策略
    /// </summary>
    public class ConfigInfoOperator<T> where T : IConfigInfo
    {

        #region 帮助方法

        public T LoadConfigInfo()
        {
            //var tp = typeof(T);
            //string configInfoFile = tp.Name;
            //return LoadConfigInfo(configInfoFile);
            return ConfigInfoOperator.LoadConfigInfo<T>();
        }
        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="configInfoType">配置信息类型</param>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        public T LoadConfigInfo(string configInfoFile)
        {
            //if (File.Exists(configInfoFile))
            //{
            //    return (T)IOHelper.DeserializeFromXML(typeof(T), configInfoFile);
            //}
            //else
            //{
            //    throw new FileNotFoundException();
            //}
            return ConfigInfoOperator.LoadConfigInfo<T>(configInfoFile);
        }

        /// <summary>
        /// 将配置信息保存到文件中
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="configInfoFile">保存路径</param>
        /// <returns>是否保存成功</returns>
        public bool SaveConfigInfo(T configInfo, string configInfoFile)
        {
            //return IOHelper.SerializeToXml(configInfo, configInfoFile);
            return ConfigInfoOperator.SaveConfigInfo(configInfo, configInfoFile);
        }

        #endregion

    }


}
