using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;
using System.Reflection;
using System.IO;

namespace MyPrismDome
{
    public class Bootstrapper : UnityBootstrapper
    {

        /// <summary>
        /// 创建主窗体
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;

            App.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            string programName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            if (programName.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
            {
                programName = programName.Substring(0, programName.Length - 4);
            }
            this.ModuleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri(string.Format("{0};component/ModuleConfig.xaml", programName), UriKind.Relative));

            //FileStream stream = new FileStream(@"F:\Project\MyTest\MyTest\MyPrismDome\ModuleConfig.xaml",FileMode.Open);
            //this.ModuleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(stream);
            //stream.Dispose();

            //ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;

            //moduleCatalog.AddModule(typeof(MyPrismDomeView.NavigationModule));

        }
    }
}
