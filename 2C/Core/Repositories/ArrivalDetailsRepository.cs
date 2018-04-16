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
        internal ArrivalDetailsRepository(string connectionString) : base(connectionString)
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
    }
}
