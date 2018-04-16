using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class OrderDetailsRepository : BaseRepository<OrderDetails>
    {
        internal OrderDetailsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<List<OrderDetails>> GetByArrivalId(int orderId)
        {
            var parameters = new List<SqlParameter>();

            var param = new SqlParameter("@orderId", orderId);

            var query = $"{GetSimpleQuery()} WHERE {ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))}={param.ParameterName}";

            parameters.Add(param);

            var res = await QueryToModelList(query, parameters);

            return res;
        }
    }
}
