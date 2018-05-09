using Core;
using Core.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class GoodsEditViewModel : VmAdapter<Goods>
    {
        private int _amount;
        private decimal _price;
        private string _name;
        private string _image;

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

        public override void ChangeModel()
        {
            Model.Name = Name;
            Model.Image = Image;
        }

        public override void ResetChanges()
        {
            Name = Model.Name;
            Image = Model.Image;
        }
    }
}
