using Core.Models;
using Prism.Commands;
using System;

namespace GUI.BaseViewModels
{
    public abstract class VmAdapter<TModel> : VmDialog where TModel : BaseModel
    {
        internal VmAdapter(TModel model)
        {
            Model = model;
            ResetChanges();
            SaveCommand = new DelegateCommand(SaveExecute, CanSave);
        }

        public VmAdapter() : this(Activator.CreateInstance<TModel>()) { }

        public TModel Model { get; private set; }

        public DelegateCommand SaveCommand { get; }

        public void LoadModel(TModel model)
        {
            Model = model;
            ResetChanges();
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

        public abstract void ChangeModel();

        public abstract void ResetChanges();
    }
}
