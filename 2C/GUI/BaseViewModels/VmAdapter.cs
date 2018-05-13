using Core.Models;
using Prism.Commands;
using System;

namespace GUI.BaseViewModels
{
    public abstract class VmAdapter<TModel> : VmDialog where TModel : BaseModel
    {
        private bool isReadOnly;

        internal VmAdapter(TModel model)
        {
            Model = model;
            Init();
            ResetChanges();
            SaveCommand = new DelegateCommand(SaveExecute, CanSave);
        }

        public VmAdapter() : this(Activator.CreateInstance<TModel>()) { }

        public TModel Model { get; private set; }

        public DelegateCommand SaveCommand { get; }
        public bool IsReadOnly { get => isReadOnly; set => SetProperty(ref isReadOnly, value); }

        public void LoadModel(TModel model)
        {
            if (model == null)
                return;
            SetModel(model);
            ResetChanges();
        }

        public void SetModel(TModel model)
        {
            Model = model;
        }

        protected virtual bool CanSave()
        {
            return Model != null;
        }

        protected virtual void SaveExecute()
        {
            ChangeModel();
            DialogResult = true;
            Close();
        }

        protected virtual void Init()
        {

        }

        public abstract void ChangeModel();

        public abstract void ResetChanges();
    }
}
