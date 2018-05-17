using Core.Models;
using GUI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class MonthReportViewModel : VmBase
    {
        private int _month;
        private decimal _arrivalsTotal;
        private decimal _ordersTotal;
        
        public MonthReportViewModel()
        {

        }

        public int Month
        {
            get => _month;
            set
            {
                SetProperty(ref _month, value);
                ChangeTitle();
            }
        }

        public decimal ArrivalsTotal
        {
            get => _arrivalsTotal;
            set
            {
                SetProperty(ref _arrivalsTotal, value);
                RaisePropertyChanged(nameof(Total));
            }
        }

        public decimal OrdersTotal
        {
            get => _ordersTotal;
            set
            {
                SetProperty(ref _ordersTotal, value);
                RaisePropertyChanged(nameof(Total));
            }
        }
        public decimal Total => OrdersTotal - ArrivalsTotal;

        private void ChangeTitle()
        {
            if (Month < 1 || Month > 12)
                return;

            Title = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        }
    }
}
