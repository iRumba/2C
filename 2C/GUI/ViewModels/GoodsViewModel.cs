using Core;
using Core.Models;
using Core.Repositories;
using GUI.BaseViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class GoodsViewModel : VmDictionary<Goods>
    {
        private Task _dataLoading;

        public GoodsViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Справочник товаров";
        }

        public override async Task LoadData()
        {
            _dataLoading = base.LoadData().ContinueWith(t => LoadDetails());
            await _dataLoading;
        }

        public async Task WaitForDataLoading()
        {
            if (_dataLoading == null)
                return;
            await _dataLoading;
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
