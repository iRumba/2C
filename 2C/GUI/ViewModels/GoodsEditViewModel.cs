using Core.Models;
using GUI.BaseViewModels;
using Prism.Commands;
using System;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;

namespace GUI.ViewModels
{
    public class GoodsEditViewModel : VmAdapter<Goods>
    {
        private int _amount;
        private decimal _price;
        private string _name;
        private string _image;
        private double _markUp;
        private string _imagesPath = "BarCodes";

        public GoodsEditViewModel() 
        {
            ChangeImageCommand = new DelegateCommand(ChangeImage);
            DeleteImageCommand = new DelegateCommand(DeleteImage, CanDeleteImage);
            Title = "Редактирование товара";
        }

        public DelegateCommand ChangeImageCommand { get; }
        public DelegateCommand DeleteImageCommand { get; }

        bool CanDeleteImage()
        {
            return !string.IsNullOrEmpty(Image);
        }

        void ChangeImage()
        {
            
        }

        void DeleteImage()
        {
            Image = null;
        }

        public int Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public double MarkUp { get => _markUp; set => SetProperty(ref _markUp, value); }

        public override void ChangeModel()
        {
            Model.Name = Name;
            //Model.Image = Image;
            Model.Markup = MarkUp / 100;
        }

        public override void ResetChanges()
        {
            Name = Model.Name;
            Image = GetBarCodeFileName();
            MarkUp = Model.Markup * 100;
        }

        private string GetBarCodeFileName()
        {
            var barCode = GetBarCode();
            var filePath = $@"{_imagesPath}\{barCode}.png";
            if (!File.Exists(filePath))
                filePath = SaveBarToFile();

            if (!string.IsNullOrWhiteSpace(filePath))
                filePath = Path.GetFullPath(filePath);

            return filePath;
        }

        private string GetBarCode()
        {
            if (Model.Id < 1)
                return null;

            var barId = ReverseString($"{Model.Id}").PadRight(12, '0');
            var buffers = new int[2];

            for(var i = 0; i < 12; i++)
            {
                var reversedI = 11 - i;
                var digit = (int)char.GetNumericValue(barId[reversedI]);
                buffers[i % 2] += digit;
            }

            var controlSum = buffers[0] * 3 + buffers[1];
            var lastDigit = controlSum % 10;
            var res = 10 - lastDigit;
            return $"{barId}{res % 10}";
        }

        private string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private string SaveBarToFile()
        {
            try
            {
                if (!Directory.Exists(_imagesPath))
                    Directory.CreateDirectory(_imagesPath);

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

                var barCode = GetBarCode();

                var bmp = w.Write(barCode);

                var fileName = $@"{_imagesPath}\{barCode}.png";
                bmp.Save(fileName, ImageFormat.Png);
                return fileName;
            }
            catch
            {
                return null;
            }
        }
    }
}
