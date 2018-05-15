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
            var conf = ConfigManager.LoadFromFile("constr.txt");
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

            Container.Register<VmAdapter<Arrival>, ArrivalEditViewModel>();
            Container.Register<EditView<Arrival>, ArrivalEditView>();
            Container.Register<DictionaryView<Arrival>, ArrivalsView>();
            Container.Register<EditView<ArrivalDetails>, ArrivalDetailEditView>();

            Container.Register<VmAdapter<Order>, OrderEditViewModel>();
            Container.Register<EditView<Order>, OrderEditView>();
            Container.Register<DictionaryView<Order>, OrdersView>();
            Container.Register<EditView<OrderDetails>, OrderDetailEditView>();
        }
    }
}
