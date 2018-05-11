using GUI.BaseViewModels;
using System;
using System.Windows;

namespace GUI.BaseViews
{
    public class BaseView<TViewModel> : Window where TViewModel : VmBase
    {
        public BaseView()
        {
            Loaded += BaseView_Loaded;
        }

        public TViewModel ViewModel
        {
            get
            {
                return DataContext as TViewModel;
            }
        }

        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseRequest += ViewModel_CloseRequest;
        }

        private void ViewModel_CloseRequest(object sender, EventArgs e)
        {
            RaiseClose();
        }

        protected virtual void RaiseClose()
        {
            Close();
        }
    }
}
