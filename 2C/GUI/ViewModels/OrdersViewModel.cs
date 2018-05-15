using Core;
using Core.Models;
using Core.Repositories;
using GUI.BaseViewModels;
using GUI.BaseViews;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.ViewModels
{
    public class OrdersViewModel : VmDictionary<Order>
    {
        public OrdersViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Список заказов";
        }

        public override async Task LoadData()
        {
            await base.LoadData();
            await LoadDetails();
        }

        private async Task LoadDetails()
        {
            var detailsRep = ShopManager.RepositoryManager.GetRepository<OrderDetails>() as OrderDetailsRepository;
            var purchasersRep = ShopManager.RepositoryManager.GetRepository<Purchaser>();
            var workersRep = ShopManager.RepositoryManager.GetRepository<Worker>();
            foreach (var entity in Entities)
            {
                var details = await detailsRep.GetByOrderId(entity.Model.Id);
                entity.Model.OrderDetails = details;
                var purchaser = await purchasersRep.GetById(entity.Model.GetForeignKey(nameof(entity.Model.Purchaser)));
                entity.Model.Purchaser = purchaser;
                var worker = await workersRep.GetById(entity.Model.GetForeignKey(nameof(entity.Model.Worker)));
                entity.Model.Worker = worker;
                entity.ResetChanges();
            }
        }

        protected override void BeforeEditExecute(VmAdapter<Order> viewModel)
        {
            viewModel.IsReadOnly = true;
        }

        protected override async void AfterAddExecute(VmAdapter<Order> viewModel)
        {
            var rep = ShopManager.RepositoryManager.GetRepository<OrderDetails>();
            if (viewModel.Model.OrderDetails == null)
                return;
            await rep.Add(viewModel.Model.OrderDetails);
        }

        protected override void AfterEditExecute(VmAdapter<Order> viewModel)
        {
            base.AfterEditExecute(viewModel);
        }


    }
}
