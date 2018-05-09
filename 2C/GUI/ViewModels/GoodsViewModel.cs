using Core;
using Core.Models;
using Core.Repositories;
using GUI.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class GoodsViewModel : VmDictionary<Goods>
    {
        public GoodsViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Справочник товаров";
        }

        public override async Task LoadData()
        {
            await base.LoadData();
            await LoadDetails();
        }

        private async Task LoadDetails()
        {
            var rep = ShopManager.RepositoryManager.GetRepository<Goods>() as GoodsRepository;
            if (rep == null)
                return;

            var details = await rep.GetDetails(Entities.Select(g => g.Model.Id));

            foreach(var det in details)
            {
                var g = Entities.FirstOrDefault(gvm => gvm.Model.Id == det.Item1) as GoodsEditViewModel;
                g.Price = det.Item2 ?? 0;
                g.Amount = det.Item3;
            }
        }
    }
}
