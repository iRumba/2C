using Core;
using Core.Models;
using GUI.BaseViewModels;

namespace GUI.ViewModels
{
    public class WorkersViewModel : VmDictionary<Worker>
    {
        public WorkersViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Список работников";
        }
    }
}
