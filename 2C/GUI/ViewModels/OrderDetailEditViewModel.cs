using Core;
using Core.Models;
using Core.Repositories;
using GUI.BaseViewModels;
using GUI.Helpers;
using GUI.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class OrderDetailEditViewModel : VmAdapter<OrderDetails>, IShopWorker
    {
        private GoodsEditViewModel _goods;
        private OrderEditViewModel _order;
        private int _amount;
        
        internal OrderDetailEditViewModel() : this(null) { }

        public OrderDetailEditViewModel(ShopManager shopManager)
        {
            ShopManager = shopManager;
            Title = "Редактирование деталей заказа";
            SelectGoodsCommand = new DelegateCommand<GoodsEditViewModel>(SelectHelper.Assign);
        }

        protected override void Init()
        {
            Goods = new GoodsEditViewModel();
            Goods.PropertyChanged += Goods_PropertyChanged;
            Order = new OrderEditViewModel();
        }

        private async void Goods_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Goods.Price))
            {
                RaisePropertyChanged(nameof(Price));
                RaisePropertyChanged(nameof(Total));
            }
            else
            {
                await LoadGoodsDetails();
            }
        }

        public DelegateCommand<GoodsEditViewModel> SelectGoodsCommand { get; }

        public GoodsEditViewModel Goods { get => _goods; private set => SetProperty(ref _goods, value); }
        public OrderEditViewModel Order { get => _order; private set => SetProperty(ref _order, value); }

        public int Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
                RaisePropertyChanged(nameof(Total));
            }
        }

        public decimal Price => Goods.Price;

        public decimal Total => Amount * Price;

        public ShopManager ShopManager { get; private set; }

        public override void ChangeModel()
        {
            Model.Amount = Amount;
            Model.Price = Price;
            Goods.ChangeModel();
            Model.Goods = Goods.Model;
            Model.Order = Order.Model;
        }

        public override void ResetChanges()
        {
            Amount = Model.Amount;
            Goods.LoadModel(Model.Goods);
            Goods.Price = Model.Price;
            Order.SetModel(Model.Order);
        }

        private async Task LoadGoodsDetails()
        {
            if (Goods == null || Goods.Model.Id == 0 || ShopManager == null)
                return;

            var goodsRep = ShopManager.RepositoryManager.GetRepository<Goods>() as GoodsRepository;

            var det = await goodsRep.GetDetails(Goods.Model.Id);
            Goods.Price = det.Item1 ?? 0;
            Goods.Amount = det.Item2;
        }
    }
}
