using GUI.Views;
using System.Windows;
using Prism.Modularity;
using DryIoc;
using Prism.DryIoc;
using Microsoft.Practices.ServiceLocation;
using Core;
using GUI.ViewModels;
using Core.Models;
using GUI.BaseViews;
using GUI.BaseViewModels;

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
            Container.RegisterInstance(conf);
            Container.Register<ShopManager>();

            Container.Register<VmAdapter<Goods>, GoodsEditViewModel>();
            Container.Register<EditView<Goods>, GoodsEditView>();
            Container.Register<DictionaryView<Goods>, GoodsView>();

            Container.Register<VmAdapter<Worker>, WorkersEditViewModel>();
            Container.Register<EditView<Worker>, WorkersEditView>();
            Container.Register<DictionaryView<Worker>, WorkersView>();

            Container.Register<VmAdapter<Purveyor>, PurveyorEditViewModel>();
            Container.Register<EditView<Purveyor>, PurveyorEditView>();
            Container.Register<DictionaryView<Purveyor>, PurveyorsView>();

            Container.Register<VmAdapter<Purchaser>, PurchaserEditViewModel>();
            Container.Register<EditView<Purchaser>, PurchaserEditView>();
            Container.Register<DictionaryView<Purchaser>, PurchasersView>();
        }
    }
}
