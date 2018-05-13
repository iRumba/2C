using Core.Models;
using GUI.BaseViewModels;
using GUI.BaseViews;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpers
{
    static class SelectHelper
    {
        public static async Task<TModel> Change<TModel>(TModel selected) where TModel : BaseModel
        {
            var dictView = ServiceLocator.Current.GetInstance<DictionaryView<TModel>>();
            var vm = dictView.ViewModel;
            if (selected != null)
                vm.StartId = selected.Id;
            vm.IsDialog = true;
            await vm.LoadData();
            if (dictView.ShowDialog() == true)
                return dictView.ViewModel.Selected.Model;

            return null;
        }

        public static async void Assign<TModel>(VmAdapter<TModel> property) where TModel : BaseModel
        {
            var selected = await Change(property.Model);
            if (selected == null)
                return;

            property.LoadModel(selected);
        }
    }
}
