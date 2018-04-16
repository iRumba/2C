using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    internal static class AdaptersManager
    {
        static AdaptersManager()
        {
        }

        internal static BaseAdapter<TModel> GetAdapter<TModel>() where TModel : BaseModel
        {
            return new BaseAdapter<TModel>();// (BaseAdapter<TModel>)_adapters[typeof(TModel)];
        }
    }
}
