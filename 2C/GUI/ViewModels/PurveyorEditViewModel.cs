using Core.Models;
using GUI.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class PurveyorEditViewModel : VmAdapter<Purveyor>
    {
        private string _name;
        private string _telephoneNumber;

        public PurveyorEditViewModel()
        {
            Title = "Редактирование поставщика";
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string TelephoneNumber { get => _telephoneNumber; set => SetProperty(ref _telephoneNumber, value); }

        public override void ChangeModel()
        {
            Model.Name = Name;
            Model.TelephoneNumber = TelephoneNumber;
        }

        public override void ResetChanges()
        {
            Name = Model.Name;
            TelephoneNumber = Model.TelephoneNumber;
        }
    }
}
