using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.BaseViews
{
    public class BaseView<TViewModel> : Window where TViewModel : VmDialog
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
            DialogResult = ViewModel.DialogResult;
            Close();
        }
    }
}
