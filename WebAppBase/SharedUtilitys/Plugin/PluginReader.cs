using SharedUtilitys.ConfigInfo;
using SharedUtilitys.Helper;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharedUtilitys.Plugin
{
    public class PluginManifest : IConfigInfo
    {
        public string Ver { get; set; }
        public List<PluginInfo> PluginInfos { get; set; }

        public IPlugin GetPlugin(string PluginName)
        {
            var info = PluginInfos.Find(m => m.PluginName.Equals(PluginName));
            if (info == null)
            {
                throw new InvalidDataException();
            }
            return GetPlugin(info);
        }

        public T GetPlugin<T>() where T : IPlugin
        {
            string PluginName = typeof(T).Name;
            return GetPlugin<T>(PluginName);
        }

        public T GetPlugin<T>(string PluginName) where T : IPlugin
        {
            //ISMSPlugin
            var tryNames = new List<string>();
            tryNames.Add(PluginName.ToLower());
            if (PluginName.StartsWith("I"))
            {
                var s1 = PluginName.Substring(1);
                tryNames.Add(s1.ToLower());

                if (s1.EndsWith("Plugin"))
                {
                    s1 = s1.Substring(0, s1.Length - 8);
                    tryNames.Add(s1.ToLower());
                }
            }
            foreach (var n in tryNames)
            {
                var info = PluginInfos.Find(m => m.PluginName.ToLower().Equals(n));
                if (info != null)
                {
                    return (T)GetPlugin(info);
                }
            }

            throw new ArgumentException();
        }

        public static IPlugin GetPlugin(PluginInfo info)
        {
            try
            {
                var fileName = Path.Combine(System.Web.HttpRuntime.BinDirectory, string.Format("{0}", info.FileName));
                var plugin = (IPlugin)Activator.CreateInstance(Type.GetType(string.Format("{0}, {1}", info.ClassName, info.AssemblyName), false, true));
                return plugin;
            }
            catch
            {
                throw new PluginCreateFailedException(info.PluginName);
            }
        }
    }

    public class PluginLoader : ConfigInfoOperator<PluginManifest>
    {
        #region 私有字段
        private const string _strategies = "~/App_Data/pluginmanifest.config";
        #endregion
        /// <summary>
        /// 获得关系数据库配置
        /// </summary>
        /// 
        public PluginManifest GetPluginManifest()
        {
            try
            {
                return LoadConfigInfo(IOHelper.GetMapPath(_strategies));
            }
            catch (FileNotFoundException)
            {
                return new PluginManifest() { Ver = "", PluginInfos = new List<PluginInfo>() };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
