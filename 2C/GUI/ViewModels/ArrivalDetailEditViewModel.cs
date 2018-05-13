using Core.Models;
using GUI.BaseViewModels;
using GUI.Helpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class ArrivalDetailEditViewModel : VmAdapter<ArrivalDetails>
    {
        private int _amount;
        private decimal _price;
        private GoodsEditViewModel _goods;
        private ArrivalEditViewModel _arrival;

        public ArrivalDetailEditViewModel()
        {

            Title = "Редактирование деталей поставок";
            SelectGoodsCommand = new DelegateCommand<GoodsEditViewModel>(SelectHelper.Assign);
        }

        protected override void Init()
        {
            Goods = new GoodsEditViewModel();
            Arrival = new ArrivalEditViewModel();
        }

        public DelegateCommand<GoodsEditViewModel> SelectGoodsCommand { get; }

        public int Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
                RaisePropertyChanged(nameof(Total));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                RaisePropertyChanged(nameof(Total));
            }
        }

        public GoodsEditViewModel Goods { get => _goods; private set => SetProperty(ref _goods, value); }
        public ArrivalEditViewModel Arrival { get => _arrival; private set => SetProperty(ref _arrival, value); }
        public decimal Total => Amount * Price;

        public override void ChangeModel()
        {
            Model.Amount = Amount;
            Model.Price = Price;
            Goods.ChangeModel();
            Model.Goods = Goods.Model;
            Model.Arrival = Arrival.Model;
        }

        public override void ResetChanges()
        {
            Amount = Model.Amount;
            Price = Model.Price;
            Goods.LoadModel(Model.Goods);
            Arrival.SetModel(Model.Arrival);
        }
    }
}
