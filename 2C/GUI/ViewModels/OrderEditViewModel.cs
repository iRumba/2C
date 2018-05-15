using Core.Models;
using GUI.BaseViewModels;
using GUI.BaseViews;
using GUI.Helpers;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.ViewModels
{
    public class OrderEditViewModel : VmAdapter<Order>
    {
        private DateTime _orderDate;
        private DateTime? _departureDate;
        private PurchaserEditViewModel _purchaser;
        private DateTime? _arrivalDate;
        private string _deliveryMethod;
        private string _paymentMethod;
        private WorkersEditViewModel _worker;

        public OrderEditViewModel()
        {
            AddDetailCommand = new DelegateCommand(AddDetail);
            EditDetailCommand = new DelegateCommand<OrderDetailEditViewModel>(EditDetail, CanEdit);
            SelectPurchaserCommand = new DelegateCommand<PurchaserEditViewModel>(SelectHelper.Assign);
            SelectWorkerCommand = new DelegateCommand<WorkersEditViewModel>(SelectHelper.Assign);
            RemoveDetailCommand = new DelegateCommand<OrderDetailEditViewModel>(RemoveDetail, CanRemove);
            OrderDate = DateTime.Now;
        }

        protected override void Init()
        {
            Purchaser = new PurchaserEditViewModel();
            Worker = new WorkersEditViewModel();
            Details = new ObservableCollection<OrderDetailEditViewModel>();
            Details.CollectionChanged += Details_CollectionChanged;
        }

        private void Details_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Total));
        }

        public ObservableCollection<OrderDetailEditViewModel> Details { get; private set; }

        public DateTime OrderDate { get => _orderDate; set => SetProperty(ref _orderDate, value); }
        public DateTime? DepartureDate { get => _departureDate; set => SetProperty(ref _departureDate, value); }
        public PurchaserEditViewModel Purchaser { get => _purchaser; private set => SetProperty(ref _purchaser, value); }
        public DateTime? ArrivalDate { get => _arrivalDate; set => SetProperty(ref _arrivalDate, value); }
        public string DeliveryMethod { get => _deliveryMethod; set => SetProperty(ref _deliveryMethod, value); }
        public string PaymentMethod { get => _paymentMethod; set => SetProperty(ref _paymentMethod, value); }
        public WorkersEditViewModel Worker { get => _worker; set => SetProperty(ref _worker, value); }

        public DelegateCommand AddDetailCommand { get; }
        public DelegateCommand<PurchaserEditViewModel> SelectPurchaserCommand { get; }
        public DelegateCommand<WorkersEditViewModel> SelectWorkerCommand { get; }
        public DelegateCommand<OrderDetailEditViewModel> EditDetailCommand { get; }
        public DelegateCommand<OrderDetailEditViewModel> RemoveDetailCommand { get; }

        public string[] DeliveryMethods { get; } = new[]
        {
            "Доставка",
            "Самовывоз"
        };

        public string[] PaymentMethods { get; } = new[]
{
            "Наличными",
            "По карте"
        };

        public decimal Total => Details.Sum(d => d.Total);

        public override void ChangeModel()
        {
            if (OrderDate == default(DateTime))
                OrderDate = DateTime.Now;
            Model.OrderDate = OrderDate;
            Purchaser.ChangeModel();
            Model.Purchaser = Purchaser.Model;
            Worker.ChangeModel();
            Model.Worker = Worker.Model;
            Model.PaymentMethod = PaymentMethod;
            Model.ArrivalDate = ArrivalDate;
            Model.DeliveryMethod = DeliveryMethod;
            Model.DepartureDate = DepartureDate;

            Model.OrderDetails = new List<OrderDetails>();
            foreach (var det in Details)
            {
                det.Order.LoadModel(Model);
                det.ChangeModel();
                Model.OrderDetails.Add(det.Model);
            }
        }

        public override void ResetChanges()
        {
            OrderDate = Model.OrderDate;
            PaymentMethod = Model.PaymentMethod;
            ArrivalDate = Model.ArrivalDate;
            DeliveryMethod = Model.DeliveryMethod;
            DepartureDate = Model.DepartureDate;
            Purchaser.LoadModel(Model.Purchaser);
            Worker.LoadModel(Model.Worker);

            Details.Clear();
            if (Model.OrderDetails == null)
                return;
            foreach (var det in Model.OrderDetails)
            {
                var detVm = new OrderDetailEditViewModel();
                detVm.LoadModel(det);
                Details.Add(detVm);
            }
        }

        private void AddDetail()
        {
            var editView = ServiceLocator.Current.GetInstance<EditView<OrderDetails>>();
            var vm = editView.ViewModel;
            vm.Title = $"{vm.Title} [новый]";
            editView.ShowDialog();
            if (vm.DialogResult == true)
            {
                if (Details.Any(d => d.Model.Goods.Id == vm.Model.Goods.Id))
                {
                    MessageBox.Show("Данный товар уже есть в списке", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var newDetail = new OrderDetailEditViewModel();
                    vm.Model.Order = Model;
                    newDetail.LoadModel(vm.Model);
                    newDetail.Goods.Price = vm.Model.Price;
                    Details.Add(newDetail);
                }
            }
        }

        private bool CanEdit(OrderDetailEditViewModel selected)
        {
            return selected != null;
        }

        private void EditDetail(OrderDetailEditViewModel selected)
        {
            var editView = ServiceLocator.Current.GetInstance<EditView<OrderDetails>>();
            var vm = editView.ViewModel;
            vm.LoadModel(selected.Model);
            editView.ShowDialog();
            if (vm.DialogResult == true)
            {
                if (Details.Any(d => d.Model.Goods.Id == vm.Model.Goods.Id))
                {
                    MessageBox.Show("Данный товар уже есть в списке", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    selected.LoadModel(vm.Model);
                }
            }
        }

        private void RemoveDetail(OrderDetailEditViewModel selected)
        {
            if (selected == null)
                return;
            Details.Remove(selected);
        }

        private bool CanRemove(OrderDetailEditViewModel selected)
        {
            return selected != null;
        }
    }
}
