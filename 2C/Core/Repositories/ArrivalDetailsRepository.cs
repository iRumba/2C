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
    public class ArrivalDetailsRepository : BaseRepository<ArrivalDetails>
    {
        public ArrivalDetailsRepository(DbManager dbManager) : base(dbManager)
        {
        }

        public async Task<List<ArrivalDetails>> GetByArrivalId(int arrivalId)
        {
            var parameters = new List<SqlParameter>();

            var param = new SqlParameter("@arrivalId", arrivalId);

            var query = $"{GetSimpleQuery()} WHERE {ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))}={param.ParameterName}";

            parameters.Add(param);

            var res = await QueryToModelList(query, parameters);

            return res;
        }

        public async Task<List<ArrivalDetails>> GetByArrivalId(IEnumerable<int> arrivalIds)
        {
            var parameters = new List<SqlParameter>();

            var paramNames = new List<string>();
            foreach (var arrivalId in arrivalIds.Select((id, i) => new { Id = id, Ind = i }))
            {
                var param = new SqlParameter($"@arrivalId{arrivalId.Ind}", arrivalId.Id);
                paramNames.Add(param.ParameterName);
                parameters.Add(param);
            }

            var query = $"{GetSimpleQuery()} WHERE {ModelHelper.GetColumnName<ArrivalDetails>(nameof(ArrivalDetails.Arrival))} IN ({string.Join(",", paramNames)})";

            var res = await QueryToModelList(query, parameters);

            return res;
        }
    }
}
