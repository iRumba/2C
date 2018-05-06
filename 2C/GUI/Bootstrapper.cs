using GUI.Views;
using System.Windows;
using Prism.Modularity;
using DryIoc;
using Prism.DryIoc;
using Microsoft.Practices.ServiceLocation;
using Core;

namespace GUI
{
    class Bootstrapper : DryIocBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(YOUR_MODULE));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            var conf = ConfigManager.GetDefault(); 
            Container.Register<ShopManager>();
        }
    }
}
