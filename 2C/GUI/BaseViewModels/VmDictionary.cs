using Core;
using Core.Models;
using GUI.BaseViews;
using GUI.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GUI.BaseViewModels
{
    public class VmDictionary<TModel> : VmDialog, IShopWorker where TModel : BaseModel
    {
        private VmAdapter<TModel> _selected;

        public DelegateCommand SelectCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand AddCommand { get; }

        public VmDictionary(ShopManager shopManager)
        {
            Entities = new ObservableCollection<VmAdapter<TModel>>();
            SelectCommand = new DelegateCommand(SelectExecute, IsSelected).ObservesProperty(() => Selected);
            EditCommand = new DelegateCommand(EditExecute, IsSelected).ObservesProperty(() => Selected);
            AddCommand = new DelegateCommand(AddExecute);
            ShopManager = shopManager;
        }

        public ObservableCollection<VmAdapter<TModel>> Entities { get; }

        public int StartId { get; set; }

        public VmAdapter<TModel> Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ShopManager ShopManager { get; }

        void SelectExecute()
        {
            DialogResult = true;
            Close();
        }

        public virtual async Task LoadData()
        {
            var entities = await ShopManager.RepositoryManager.GetRepository<TModel>().GetAll();
            foreach(var entity in entities)
            {
                var adapter = ServiceLocator.Current.GetInstance<VmAdapter<TModel>>();
                adapter.LoadModel(entity);
                Entities.Add(adapter);
                if (entity.Id == StartId)
                    Selected = adapter;
            }
        }

        bool IsSelected()
        {
            return Selected != null;
        }

        async void AddExecute()
        {
            var editView = ServiceLocator.Current.GetInstance<EditView<TModel>>();
            var vm = editView.ViewModel;
            vm.Title = $"{vm.Title} [новый]";
            BeforeAddExecute(vm);
            editView.ShowDialog();
            if (vm.DialogResult == true)
            {
                var rep = ShopManager.RepositoryManager.GetRepository<TModel>();
                vm.ChangeModel();
                var newModel = await rep.Add(vm.Model);
                vm.Model.Id = newModel.Id;
                //vm.LoadModel(newModel);
                AfterAddExecute(vm);
                Entities.Insert(0, vm);
            }
        }

        async void EditExecute()
        {
            var goodsEdit = ServiceLocator.Current.GetInstance<EditView<TModel>>();
            var vm = goodsEdit.ViewModel;
            vm.LoadModel(Selected.Model);
            vm.Title = $"{vm.Title} [#{vm.Model.Id}]";
            BeforeEditExecute(vm);
            goodsEdit.ShowDialog();
            if (vm.DialogResult == true)
            {
                var rep = ShopManager.RepositoryManager.GetRepository<TModel>();
                vm.ChangeModel();
                if (await rep.Update(vm.Model))
                {
                    Selected.LoadModel(vm.Model);
                    AfterEditExecute(vm);
                }
            }
        }

        protected virtual void BeforeAddExecute(VmAdapter<TModel> viewModel)
        {

        }

        protected virtual void BeforeEditExecute(VmAdapter<TModel> viewModel)
        {

        }

        protected virtual void AfterAddExecute(VmAdapter<TModel> viewModel)
        {

        }

        protected virtual void AfterEditExecute(VmAdapter<TModel> viewModel)
        {

        }
    }
}
