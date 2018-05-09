using Core.Models;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.BaseViews
{
    public class DictionaryView<TModel> : BaseView<VmDictionary<TModel>> where TModel : BaseModel
    {

    }
}
