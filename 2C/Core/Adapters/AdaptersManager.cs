using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    public static class AdaptersManager
    {
        static Dictionary<Type, object> _adapters;

        static AdaptersManager()
        {
            _adapters = new Dictionary<Type, object>();
            AddAdapter(new GoodsAdapter());
        }

        internal static void AddAdapter<TModel>(BaseAdapter<TModel> adapter) where TModel : BaseModel
        {
            _adapters.Add(typeof(TModel), adapter);
        }

        internal static BaseAdapter<TModel> GetAdapter<TModel>() where TModel : BaseModel
        {
            return new BaseAdapter<TModel>();// (BaseAdapter<TModel>)_adapters[typeof(TModel)];
        }
    }
}
