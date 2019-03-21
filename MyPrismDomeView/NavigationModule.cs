using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Regions;
using Common;

namespace MyPrismDomeView
{
    public class NavigationModule : IModule
    {
        //private IRegionManager _regionmodule;
        public NavigationModule()
        {
            //this._regionmodule = module;
        }
        public void Initialize()
        {
            ServiceProvider.RegionManager.RegisterViewWithRegion("navigationCotrol", typeof(View.Navigation));
            
        }

        //public NavigationModule()
        //{

        //}
        //public void Initialize()
        //{
            
        //}
    }
}
