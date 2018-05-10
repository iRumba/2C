using Prism.Mvvm;

namespace GUI.BaseViewModels
{
    public class VmBase : BindableBase
    {
        private string _title = "Товары";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
