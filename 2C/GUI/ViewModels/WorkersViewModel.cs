using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
