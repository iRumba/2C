using System;

namespace GUI.BaseViewModels
{
    public class VmDialog : VmBase
    {
        private bool _isDialog;
        private bool? _dialogResult;

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

    }
}
