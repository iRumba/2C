using GUI.Views;
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
            ShowGoodsCommand = new DelegateCommand(ShowGoods);
        }

        public DelegateCommand ShowGoodsCommand { get; set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        async void ShowGoods()
        {
            var goodsView = ServiceLocator.Current.GetInstance<Goods>();
            var task = goodsView.ViewModel.LoadData();
            goodsView.Show();
            await task;
        }
    }
}
