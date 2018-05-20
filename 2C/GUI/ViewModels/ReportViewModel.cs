using Core;
using Core.Models;
using Core.Repositories;
using GUI.BaseViewModels;
using GUI.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class ReportViewModel : VmBase, IShopWorker
    {
        private int? _year;
        private decimal _total;

        public ReportViewModel(ShopManager shopManager)
        {
            Title = "Годовой отчет";
            ShopManager = shopManager;
            CreateReportCommand = new DelegateCommand<int?>(CreateReport, CanCreateRepoprt).ObservesProperty(() => Year);
            MonthList = new SortedList<int, MonthReportViewModel>();
            for (var i = 0; i < 12; i++)
            {
                MonthList[i] = new MonthReportViewModel { Month = i + 1 };
            }
        }

        public ShopManager ShopManager { get; private set; }

        public SortedList<int, MonthReportViewModel> MonthList { get; }

        public DelegateCommand<int?> CreateReportCommand { get; }

        public int? Year { get => _year; set => SetProperty(ref _year, value); }
        public decimal Total { get => _total; set => SetProperty(ref _total, value); }

        private bool CanCreateRepoprt(int? year)
        {
            return year.HasValue && year > 0 && year < 10000;
        }

        private void ClearMonthList()
        {
            for (var i = 0; i < 12; i++)
            {
                MonthList[i].ArrivalsTotal = 0;
                MonthList[i].OrdersTotal = 0;
            }
        }

        private async void CreateReport(int? y)
        {
            ClearMonthList();
            var year = y.Value;
            var fromDate = new DateTime(year, 1, 1);
            var toDate = year < 9999 ? new DateTime(year + 1, 1, 1) : new DateTime(year, 12, 31);

            var arrivalsRep = ShopManager.RepositoryManager.GetRepository<Arrival>() as ArrivalRepository;
            var ordersRep = ShopManager.RepositoryManager.GetRepository<Order>() as OrderRepository;

            var arrivals = await arrivalsRep.GetBetweenDates(fromDate, toDate);
            var orders = await ordersRep.GetBetweenDates(fromDate, toDate);

            var arrivalDetailsRep = ShopManager.RepositoryManager.GetRepository<ArrivalDetails>() as ArrivalDetailsRepository;
            foreach (var arrival in arrivals)
            {
                var details = await arrivalDetailsRep.GetByArrivalId(arrival.Id);
                var month = arrival.Date.Month;
                MonthList[month - 1].ArrivalsTotal += details.Sum(d => d.Price);
            }

            var orderDetailsRep = ShopManager.RepositoryManager.GetRepository<OrderDetails>() as OrderDetailsRepository;
            foreach (var order in orders)
            {
                var details = await orderDetailsRep.GetByOrderId(order.Id);
                var month = order.OrderDate.Month;
                MonthList[month - 1].OrdersTotal += details.Sum(o => o.Price);
            }

            Total = MonthList.Sum(ml => ml.Value.Total);

            RaisePropertyChanged(nameof(MonthList));
        }
    }
}
