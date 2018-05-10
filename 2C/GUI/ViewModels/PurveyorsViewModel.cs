using Core;
using Core.Models;
using GUI.BaseViewModels;

namespace GUI.ViewModels
{
    public class PurveyorsViewModel : VmDictionary<Purveyor>
    {
        public PurveyorsViewModel(ShopManager shopManager) : base(shopManager)
        {
            Title = "Список поставщиков";
        }
    }
}
