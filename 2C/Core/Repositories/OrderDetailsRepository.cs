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
        public OrderDetailsRepository(DbManager dbManager) : base(dbManager)
        {
        }

        public async Task<List<OrderDetails>> GetByOrderId(int orderId)
        {
            var parameters = new List<SqlParameter>();

            var param = new SqlParameter("@orderId", orderId);

            var query = $"{GetSimpleQuery()} WHERE {ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))}={param.ParameterName}";

            parameters.Add(param);

            var res = await QueryToModelList(query, parameters);

            return res;
        }

        public async Task<List<OrderDetails>> GetByArrivalId(IEnumerable<int> orderIds)
        {
            var parameters = new List<SqlParameter>();

            var paramNames = new List<string>();
            foreach(var orderId in orderIds.Select((id, i) => new { Id = id, Ind = i }))
            {
                var param = new SqlParameter($"@orderId{orderId.Ind}", orderId.Id);
                paramNames.Add(param.ParameterName);
                parameters.Add(param);
            }

            var query = $"{GetSimpleQuery()} WHERE {ModelHelper.GetColumnName<OrderDetails>(nameof(OrderDetails.Order))} IN ({string.Join(",", paramNames)})";

            var res = await QueryToModelList(query, parameters);

            return res;
        }
    }
}
