using Core;
using Core.Models;
using Core.Repositories;
using GUI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class ArrivalsViewModel : VmDictionary<Arrival>
    {
        public ArrivalsViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Поставки";
        }

        public override async Task LoadData()
        {
            await base.LoadData();
            await LoadDetails();
        }

        private async Task LoadDetails()
        {
            var detailsRep = ShopManager.RepositoryManager.GetRepository<ArrivalDetails>() as ArrivalDetailsRepository;
            var purveyorsRep = ShopManager.RepositoryManager.GetRepository<Purveyor>();
            foreach (var entity in Entities)
            {
                var details = await detailsRep.GetByArrivalId(entity.Model.Id);
                entity.Model.ArrivalDetails = details;
                var purveyor = await purveyorsRep.GetById(entity.Model.GetForeignKey(nameof(entity.Model.Purveyor)));
                entity.Model.Purveyor = purveyor;
                entity.ResetChanges();
            }
        }

        protected override void BeforeEditExecute(VmAdapter<Arrival> viewModel)
        {
            viewModel.IsReadOnly = true;
        }

        protected override async void AfterAddExecute(VmAdapter<Arrival> viewModel)
        {
            var rep = ShopManager.RepositoryManager.GetRepository<ArrivalDetails>();
            if (viewModel.Model.ArrivalDetails == null)
                return;
            await rep.Add(viewModel.Model.ArrivalDetails);
        }

        protected override void AfterEditExecute(VmAdapter<Arrival> viewModel)
        {
            base.AfterEditExecute(viewModel);
        }
    }
}
