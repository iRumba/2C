using Core.Adapters;
using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class GoodsRepository : BaseRepository<Goods>
    {
        internal GoodsRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task<Tuple<decimal?, int>> GetDetails(int id)
        {
            var param = new SqlParameter("@id", id);
            var query = $"SELECT Price, Balance FROM GoodsDetails WHERE Id={param.ParameterName}";
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.Add(param);
                    using(var reader = await com.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var price = await reader.IsDBNullAsync(0) ? default(decimal?) : reader.GetDecimal(0);
                            var balance = reader.GetInt32(1);
                            var res = new Tuple<decimal?, int>(price, balance);
                            return res;
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<List<Tuple<int, decimal?, int>>> GetDetails(IEnumerable<int> ids)
        {
            var parameters = new List<SqlParameter>();
            var counter = 0;
            var sb = new StringBuilder($"SELECT Id, Price, Balance FROM GoodsDetails");
            if (ids.Any())
                sb.Append(" WHERE ");
            foreach(var id in ids)
            {
                if (counter > 0)
                    sb.Append(" OR ");
                var param = new SqlParameter($"@id_{counter}", id);
                parameters.Add(param);
                sb.Append($"Id={param.ParameterName}");
            }
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = sb.ToString();
                    com.Parameters.AddRange(parameters.ToArray());
                    using (var reader = await com.ExecuteReaderAsync())
                    {
                        var res = new List<Tuple<int, decimal?, int>>();
                        while (await reader.ReadAsync())
                        {
                            var id = await reader.GetFieldValueAsync<int>(0);
                            var price = await reader.IsDBNullAsync(0) ? default(decimal?) : await reader.GetFieldValueAsync<decimal>(1);
                            var balance = await reader.GetFieldValueAsync<int>(2);
                            res.Add(new Tuple<int, decimal?, int>(id, price, balance));
                        }
                        return res;
                    }
                }
            }
        }

        //protected override IEnumerable<string> GetFields()
        //{
        //    return base.GetFields().Concat(new string[] { "Name", "Markup", "Image" });
        //}
    }
}
