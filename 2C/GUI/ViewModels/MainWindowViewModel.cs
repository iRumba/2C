using Core.Models;
using GUI.BaseViews;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Mvvm;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Главное окно";

        public MainWindowViewModel()
        {
            ShowGoodsCommand = new DelegateCommand(ShowDictionary<Goods>);
            ShowWorkersCommand = new DelegateCommand(ShowDictionary<Worker>);
            ShowPurveyorsCommand = new DelegateCommand(ShowDictionary<Purveyor>);
            ShowPurchasersCommand = new DelegateCommand(ShowDictionary<Purchaser>);
            ShowArrivalsCommand = new DelegateCommand(ShowDictionary<Arrival>);
        }

        public DelegateCommand ShowGoodsCommand { get; }
        public DelegateCommand ShowWorkersCommand { get; }
        public DelegateCommand ShowPurveyorsCommand { get; }
        public DelegateCommand ShowPurchasersCommand { get; }
        public DelegateCommand ShowArrivalsCommand { get; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        async void ShowDictionary<TModel>() where TModel : BaseModel
        {
            var view = ServiceLocator.Current.GetInstance<DictionaryView<TModel>>();
            var task = view.ViewModel.LoadData();
            view.ShowDialog();
            await task;
        }
    }
}
