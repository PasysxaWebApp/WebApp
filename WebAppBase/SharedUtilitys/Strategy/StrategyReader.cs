using SharedUtilitys.ConfigInfo;
using SharedUtilitys.Helper;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharedUtilitys.Strategy
{
    public class StrategyManifest : IConfigInfo
    {
        public string Ver { get; set; }
        public List<StrategyInfo> StrategyInfos { get; set; }

        public IStrategy GetStrategy(string StrategyName)
        {
            var info = StrategyInfos.Find(m => m.StrategyName.Equals(StrategyName));
            if (info == null)
            {
                throw new InvalidDataException();
            }
            return GetStrategy(info);
        }

        public T GetStrategy<T>() where T : IStrategy
        {
            string StrategyName = typeof(T).Name;
            return GetStrategy<T>(StrategyName);
        }

        public T GetStrategy<T>(string StrategyName) where T : IStrategy
        {
            //ISMSStrategy
            var tryNames = new List<string>();
            tryNames.Add(StrategyName.ToLower());
            if (StrategyName.StartsWith("I"))
            {
                var s1 = StrategyName.Substring(1);
                tryNames.Add(s1.ToLower());

                if (s1.EndsWith("Strategy"))
                {
                    s1 = s1.Substring(0, s1.Length - 8);
                    tryNames.Add(s1.ToLower());
                }
            }
            foreach (var n in tryNames)
            {
                var info = StrategyInfos.Find(m => m.StrategyName.ToLower().Equals(n));
                if (info != null)
                {
                    return (T)GetStrategy(info);
                }
            }

            throw new ArgumentException();
        }

        public static IStrategy GetStrategy(StrategyInfo info)
        {
            try
            {
                var fileName = Path.Combine(System.Web.HttpRuntime.BinDirectory, string.Format("{0}", info.FileName));
                var strategy = (IStrategy)Activator.CreateInstance(Type.GetType(string.Format("{0}, {1}", info.ClassName, info.AssemblyName), false, true));
                return strategy;
            }
            catch
            {
                throw new StrategyCreateFailedException(info.StrategyName);
            }
        }
    }

    public class StrategyLoader : ConfigInfoOperator<StrategyManifest>
    {
        #region 私有字段
        private const string _strategies = "~/App_Data/strategymanifest.config";
        #endregion
        /// <summary>
        /// 获得关系数据库配置
        /// </summary>
        /// 
        public StrategyManifest GetStrategyManifest()
        {
            try
            {
                return LoadConfigInfo(IOHelper.GetMapPath(_strategies));
            }
            catch (FileNotFoundException)
            {
                return new StrategyManifest() { Ver = "", StrategyInfos = new List<StrategyInfo>() };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
