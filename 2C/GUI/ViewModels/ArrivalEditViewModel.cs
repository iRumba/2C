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
    public class ArrivalEditViewModel : VmAdapter<Arrival>
    {
        private DateTime _date;
        private PurveyorEditViewModel _purveyor;

        public ArrivalEditViewModel()
        {
            Date = DateTime.Now;
            AddDetailCommand = new DelegateCommand(AddDetail);
            EditDetailCommand = new DelegateCommand<ArrivalDetailEditViewModel>(EditDetail, CanEdit);
            SelectPurveyorCommand = new DelegateCommand<PurveyorEditViewModel>(SelectHelper.Assign);
            RemoveDetailCommand = new DelegateCommand<ArrivalDetailEditViewModel>(RemoveDetail, CanRemove);
        }

        protected override void Init()
        {
            Purveyor = new PurveyorEditViewModel();
            Details = new ObservableCollection<ArrivalDetailEditViewModel>();
            Details.CollectionChanged += Details_CollectionChanged;
        }

        private void Details_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Total));
        }

        public DelegateCommand AddDetailCommand { get; }
        public DelegateCommand<ArrivalDetailEditViewModel> EditDetailCommand { get; }
        public DelegateCommand<PurveyorEditViewModel> SelectPurveyorCommand { get; }
        public DelegateCommand<ArrivalDetailEditViewModel> RemoveDetailCommand { get; }

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }
        public PurveyorEditViewModel Purveyor { get => _purveyor; private set => SetProperty(ref _purveyor, value); }
        public decimal Total => Details.Sum(d => d.Total);

        public ObservableCollection<ArrivalDetailEditViewModel> Details { get; private set; }

        public override void ChangeModel()
        {
            if (Date == default(DateTime))
                Date = DateTime.Now;
            Model.Date = Date;
            Purveyor.ChangeModel();
            Model.Purveyor = Purveyor.Model;

            Model.ArrivalDetails = new List<ArrivalDetails>();
            foreach(var det in Details)
            {
                det.Arrival.LoadModel(Model);
                det.ChangeModel();
                Model.ArrivalDetails.Add(det.Model);
            }
        }

        public override void ResetChanges()
        {
            Date = Model.Date;
            Purveyor.LoadModel(Model.Purveyor);
            Details.Clear();
            if (Model.ArrivalDetails == null)
                return;
            foreach(var det in Model.ArrivalDetails)
            {
                var detVm = new ArrivalDetailEditViewModel();
                detVm.LoadModel(det);
                Details.Add(detVm);
            }
        }

        private bool CanEdit(ArrivalDetailEditViewModel selected)
        {
            return selected != null;
        }

        private void AddDetail()
        {
            var editView = ServiceLocator.Current.GetInstance<EditView<ArrivalDetails>>();
            var vm = editView.ViewModel;
            vm.Title = $"{vm.Title} [новый]";
            editView.ShowDialog();
            if (vm.DialogResult == true)
            {
                if (Details.Any(d=>d.Model.Goods.Id == vm.Model.Goods.Id))
                {
                    MessageBox.Show("Данный товар уже есть в списке", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var newDetail = new ArrivalDetailEditViewModel();
                    vm.Model.Arrival = Model;
                    newDetail.LoadModel(vm.Model);
                    Details.Add(newDetail);
                }
            }
        }

        private void EditDetail(ArrivalDetailEditViewModel selected)
        {
            var editView = ServiceLocator.Current.GetInstance<EditView<ArrivalDetails>>();
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

        private void RemoveDetail(ArrivalDetailEditViewModel selected)
        {
            if (selected == null)
                return;
            Details.Remove(selected);
        }

        private bool CanRemove(ArrivalDetailEditViewModel selected)
        {
            return selected != null;
        }
    }
}
