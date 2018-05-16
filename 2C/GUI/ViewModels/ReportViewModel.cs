using Core;
using GUI.BaseViewModels;
using GUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class ReportViewModel : VmBase, IShopWorker
    {
        public ReportViewModel(ShopManager shopManger)
        {
            Title = "Годовой отчет";
            ShopManager = ShopManager;
        }

        public ShopManager ShopManager { get; private set; }
    }
}
