using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public abstract class VmAdapter<TModel> : BindableBase where TModel : class, new()
    {
        public VmAdapter(TModel model)
        {
            Model = model;
            ResetChanges();
        }

        public VmAdapter()
        {
            Model = new TModel();
        }

        public TModel Model { get; private set; }

        public void LoadModel(TModel model)
        {
            Model = model;
            ResetChanges();
        }

        public abstract void ChangeModel();

        public abstract void ResetChanges();
    }
}
