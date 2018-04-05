using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    public abstract class BaseAdapter<T> where T : class
    {
        public T GetModelFromDataSet(DataSet ds)
        {
            if (ds.Tables.Count != 1)
                return null;
            var table = ds.Tables[0];
            if (table.Rows.Count == 0)
                return null;
            var row = table.Rows[0];

            return GetModelFromDataRow(row);
        }

        public List<T> GetModelListFromDataSet(DataSet ds)
        {
            if (ds.Tables.Count != 1)
                return null;

            var table = ds.Tables[0];
            var res = new List<T>();

            foreach(DataRow row in table.Rows)
            {
                res.Add(GetModelFromDataRow(row));
            }

            return res;
        }

        public async Task<T> GetModelFromDataSetAsync(DataSet ds)
        {
            Func<T> func = () => GetModelFromDataSet(ds);
            return await Task.Run(func);
        }

        public async Task<List<T>> GetModelListFromDataSetAsync(DataSet ds)
        {
            Func<List<T>> func = () => GetModelListFromDataSet(ds);
            return await Task.Run(func);
        }

        protected abstract T GetModelFromDataRow(DataRow row);
    }
}
