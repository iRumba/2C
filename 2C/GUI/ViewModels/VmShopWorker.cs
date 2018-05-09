using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class VmShopWorker : VmDialog
    {
        public VmShopWorker(ShopManager shopManager)
        {
            ShopManager = shopManager;
        }

        public ShopManager ShopManager { get; }
    }
}
