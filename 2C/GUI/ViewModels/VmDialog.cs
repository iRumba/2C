using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class VmDialog : VmBase
    {
        private bool _isDialog;
        private bool? _dialogResult;

        public event EventHandler CloseRequest;

        public bool IsDialog
        {
            get { return _isDialog; }
            set { SetProperty(ref _isDialog, value); }
        }

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetProperty(ref _dialogResult, value); }
        }

        public void Close()
        {
            CloseRequest?.Invoke(this, new EventArgs());
        }
    }
}
