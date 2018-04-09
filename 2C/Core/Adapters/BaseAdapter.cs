using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Adapters
{
    internal class BaseAdapter<TModel> where TModel : BaseModel
    {
        internal BaseAdapter()
        {

        }

        public TModel GetModelFromDataSet(DataSet ds)
        {
            if (ds.Tables.Count != 1)
                return null;
            var table = ds.Tables[0];
            if (table.Rows.Count == 0)
                return null;
            var row = table.Rows[0];

            return GetModelFromDataRow(row);
        }

        public List<TModel> GetModelListFromDataSet(DataSet ds)
        {
            if (ds.Tables.Count != 1)
                return null;

            var table = ds.Tables[0];
            var res = new List<TModel>();

            foreach(DataRow row in table.Rows)
            {
                res.Add(GetModelFromDataRow(row));
            }

            return res;
        }

        public async Task<TModel> GetModelFromDataSetAsync(DataSet ds)
        {
            Func<TModel> func = () => GetModelFromDataSet(ds);
            return await Task.Run(func);
        }

        public async Task<List<TModel>> GetModelListFromDataSetAsync(DataSet ds)
        {
            Func<List<TModel>> func = () => GetModelListFromDataSet(ds);
            return await Task.Run(func);
        }

        protected virtual TModel GetModelFromDataRow(DataRow row)
        {
            var res = Activator.CreateInstance<TModel>();
            var idField = ModelHelper.GetIdFieldName<TModel>();
            res.Id = row.Field<int>(idField);
            var mappingInfoList = ModelHelper.GetMappingInfo<TModel>();
            foreach(var mi in mappingInfoList)
            {
                if (row.Table.Columns.Contains(mi.FieldName))
                    mi.Property.SetValue(res, row.IsNull(mi.FieldName) ? null : row[mi.FieldName]);
            }
            return res;
        }
    }
}
