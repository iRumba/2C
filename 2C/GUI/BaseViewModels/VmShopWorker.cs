using Core;

namespace GUI.BaseViewModels
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
