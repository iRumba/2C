using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class VmAdapter<TModel> : BindableBase
    {
        public VmAdapter(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; }
    }
}
