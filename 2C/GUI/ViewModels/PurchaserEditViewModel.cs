using Core.Models;
using GUI.BaseViewModels;

namespace GUI.ViewModels
{
    public class PurchaserEditViewModel : VmAdapter<Purchaser>
    {
        private string _name;
        private string _telephoneNumber;
        private string _address;

        public PurchaserEditViewModel()
        {
            Title = "Редактирование клиента";
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string TelephoneNumber { get => _telephoneNumber; set => SetProperty(ref _telephoneNumber, value); }
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        public override void ChangeModel()
        {
            Model.Name = Name;
            Model.TelephoneNumber = TelephoneNumber;
            Model.Address = Address;
        }

        public override void ResetChanges()
        {
            Name = Model.Name;
            TelephoneNumber = Model.TelephoneNumber;
            Address = Model.Address;
        }
    }
}
