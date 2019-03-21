using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace Common
{
    public class ServiceProvider
    {
        //
        // 摘要:
        //     Unity容器
        public static IUnityContainer Container { get; }
        //
        // 摘要:
        //     事件聚合器
        public static IEventAggregator EventAggregator { get; }
        //
        // 摘要:
        //     区块管理器
        public static IRegionManager RegionManager { get; }
    }
}
