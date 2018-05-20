using GUI.BaseViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using ZXing;
using ZXing.Common;

namespace GUI.ViewModels
{
    public class PrintingViewModel : VmBase
    {
        public PrintingViewModel()
        {
            PrintCommand = new DelegateCommand<FlowDocument>(Print);
        }
        public DelegateCommand<FlowDocument> PrintCommand { get; }

        public void Print(FlowDocument doc)
        {
            //var pd = new PrintDialog();
            //if (pd.ShowDialog() == true)
            //{
            //    pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "printing");
            //}
            var options = new EncodingOptions
            {
                Width = 250,
                Height = 100,
                
            };
            var w = new BarcodeWriter
            {
                Options = options,
                Format = BarcodeFormat.EAN_13,
            };

            var matr = w.Write("1000000000009");
            matr.Save(@"e:\1.png", ImageFormat.Png);
        }
    }
}
