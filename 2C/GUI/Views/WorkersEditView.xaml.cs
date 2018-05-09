using Core.Models;
using GUI.BaseViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.Views
{
    /// <summary>
    /// Логика взаимодействия для WorkersEditView.xaml
    /// </summary>
    public partial class WorkersEditView : EditView<Worker>
    {
        public WorkersEditView()
        {
            InitializeComponent();
        }
    }
}
