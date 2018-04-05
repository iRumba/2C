using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class SqlCommandHelper
    {
        public static DataSet ToDataSet(this SqlCommand command)
        {
            using (var adapter = new SqlDataAdapter(command))
            {
                var res = new DataSet();
                adapter.Fill(res);
                return res;
            }
        }

        public static async Task<DataSet> ToDataSetAsync(this SqlCommand command)
        {
            Func<DataSet> func = () => ToDataSet(command);
            return await Task.Run(func);
        }
    }
}
