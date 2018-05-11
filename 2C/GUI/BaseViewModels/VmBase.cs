using Prism.Mvvm;
using System;

namespace GUI.BaseViewModels
{
    public class VmBase : BindableBase
    {
        public event EventHandler CloseRequest;

        private string _title = "Товары";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public void Close()
        {
            CloseRequest?.Invoke(this, new EventArgs());
        }
    }
}
