using GUI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class MonthReportViewModel : VmBase
    {
        private int _month;

        public ArrivalsViewModel Arrivals { get; set; }
        public int Month { get => _month; set => SetProperty(ref _month, value); }
    }
}
