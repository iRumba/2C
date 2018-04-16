using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class ArrivalRepository : BaseRepository<Arrival>
    {
        public ArrivalRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task<List<Arrival>> GetBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new List<SqlParameter>();

            if (dateFrom > dateTo)
            {
                var tmpDate = dateFrom;
                dateFrom = dateTo;
                dateTo = tmpDate;
            }

            var dateFieldName = ModelHelper.GetColumnName<Arrival>(nameof(Arrival.Date));

            var param1 = new SqlParameter("@dateFrom", dateFrom);
            var param2 = new SqlParameter("@dateTo", dateTo);

            parameters.Add(param1);
            parameters.Add(param2);

            var query = $"{GetSimpleQuery()} WHERE {dateFieldName}<={param1.ParameterName} AND {dateFieldName}={param2.ParameterName}";

            var res = await QueryToModelList(query, parameters);

            return res;
        }
    }
}
