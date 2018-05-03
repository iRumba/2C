using Core;
using Core.Models;
using Core.Repositories;
using Microsoft.Practices.ServiceLocation;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class GoodsViewModel : BindableBase
    {
        private string _title = "Товары";
        private GoodsVm _selected;

        private ShopManager _shopManager;

        //public GoodsViewModel() : this(null)
        //{
            
        //}

        public GoodsViewModel(ShopManager shopManager)
        {
            Goods = new ObservableCollection<GoodsVm>();
            _shopManager = shopManager;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<GoodsVm> Goods { get; }

        public GoodsVm Selected
        {
            get{ return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public int StartId { get; set; }

        public async Task LoadData()
        {
            var goods = await _shopManager.RepositoryManager.GetRepository<Goods>().GetAll();
            Goods.AddRange(goods.Select(g=>new GoodsVm(g)));
            if (StartId > 0)
                Selected = Goods.FirstOrDefault();
        }

        private async Task LoadDetails()
        {
            var rep = _shopManager.RepositoryManager.GetRepository<Goods>() as GoodsRepository;
            if (rep == null)
                return;

            var details = await rep.GetDetails(Goods.Select(g => g.Model.Id));

            foreach(var det in details)
            {
                var g = Goods.FirstOrDefault(gvm => gvm.Model.Id == det.Item1);
                g.Price = det.Item2 ?? 0;
                g.Amount = det.Item3;
            }
        }

        private class GoodsDetails
        {
            public GoodsDetails(int amount, decimal price)
            {

            }

            public int Amount { get; }

            public decimal Price { get; }
        }
    }
}
