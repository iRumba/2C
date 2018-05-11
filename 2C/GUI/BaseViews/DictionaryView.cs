using Core.Models;
using GUI.BaseViewModels;

namespace GUI.BaseViews
{
    public class DictionaryView<TModel> : BaseView<VmDictionary<TModel>> where TModel : BaseModel
    {
        protected override void RaiseClose()
        {
            DialogResult = ViewModel.DialogResult;
            base.RaiseClose();
        }
    }
}
