using SharedUtilitys.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core
{
    public class StrategyManager
    {
        private static StrategyManifest _strategymanifest;
        private static ISMSStrategy _ismsstrategy = null;//短信策略

        static StrategyManager()
        {
            var loader = new SharedUtilitys.Strategy.StrategyLoader();
            _strategymanifest = loader.GetStrategyManifest();
        }

        /// <summary>
        /// 短信策略实例
        /// </summary>
        public static ISMSStrategy GetSMSStrategy()
        {
            if (_ismsstrategy == null)
            {
                _ismsstrategy = _strategymanifest.GetStrategy<ISMSStrategy>();
            }
            return _ismsstrategy;

        }


    }
}
