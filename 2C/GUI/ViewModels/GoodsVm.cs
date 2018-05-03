using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class GoodsVm : VmAdapter<Goods>
    {
        private int _amount;
        private decimal _price;

        public GoodsVm(Goods model) : base(model)
        {

        }

        public int Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
    }
}
