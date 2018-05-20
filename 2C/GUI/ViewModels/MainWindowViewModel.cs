using Core;
using Core.Models;
using GUI.BaseViewModels;
using GUI.BaseViews;
using GUI.Interfaces;
using GUI.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : VmBase, IShopWorker
    {
        public MainWindowViewModel(ShopManager shopManager)
        {
            Title = "Главное окно";
            ShopManager = shopManager;
            ShowGoodsCommand = new DelegateCommand(ShowDictionary<Goods>);
            ShowWorkersCommand = new DelegateCommand(ShowDictionary<Worker>);
            ShowPurveyorsCommand = new DelegateCommand(ShowDictionary<Purveyor>);
            ShowPurchasersCommand = new DelegateCommand(ShowDictionary<Purchaser>);
            ShowArrivalsCommand = new DelegateCommand(ShowDictionary<Arrival>);
            ShowOrdersCommand = new DelegateCommand(ShowDictionary<Order>);
            CreateDbCommand = new DelegateCommand(CreateDb);
            ShowReportCommand = new DelegateCommand(ShowReport);
            ShowPrintingCommand = new DelegateCommand(ShowPrinting);
        }

        public DelegateCommand ShowGoodsCommand { get; }
        public DelegateCommand ShowWorkersCommand { get; }
        public DelegateCommand ShowPurveyorsCommand { get; }
        public DelegateCommand ShowPurchasersCommand { get; }
        public DelegateCommand ShowArrivalsCommand { get; }
        public DelegateCommand ShowOrdersCommand { get; }
        public DelegateCommand CreateDbCommand { get; }
        public DelegateCommand ShowReportCommand { get; }
        public DelegateCommand ShowPrintingCommand { get; }

        public ShopManager ShopManager { get; }

        async void ShowDictionary<TModel>() where TModel : BaseModel
        {
            var view = ServiceLocator.Current.GetInstance<DictionaryView<TModel>>();
            var task = view.ViewModel.LoadData();
            view.ShowDialog();
            await task;
        }

        async void CreateDb()
        {
            await ShopManager.CreateDatabase();
            await ShopManager.SetupDatabase();
            MessageBox.Show("База успешно создана");
        }

        private void ShowReport()
        {
            var view = ServiceLocator.Current.GetInstance<ReportView>();
            view.ViewModel.Year = DateTime.Now.Year;
            view.ViewModel.CreateReportCommand.Execute(view.ViewModel.Year);
            view.ShowDialog();
        }

        private void ShowPrinting()
        {
            var view = ServiceLocator.Current.GetInstance<PrintingView>();
            view.ShowDialog();
        }
    }
}
