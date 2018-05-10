using Core;
using Core.Models;
using GUI.BaseViewModels;

namespace GUI.ViewModels
{
    public class PurchasersViewModel : VmDictionary<Purchaser>
    {
        public PurchasersViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Список клиентов";
        }
    }
}
