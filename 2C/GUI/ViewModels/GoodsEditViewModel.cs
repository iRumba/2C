using Core.Models;
using GUI.BaseViewModels;
using Prism.Commands;

namespace GUI.ViewModels
{
    public class GoodsEditViewModel : VmAdapter<Goods>
    {
        private int _amount;
        private decimal _price;
        private string _name;
        private string _image;
        private double _markUp;

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
            Model.Image = Image;
            Model.Markup = MarkUp / 100;
        }

        public override void ResetChanges()
        {
            Name = Model.Name;
            Image = Model.Image;
            MarkUp = Model.Markup * 100;
        }
    }
}
