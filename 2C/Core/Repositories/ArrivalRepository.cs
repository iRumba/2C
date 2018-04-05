using Core.Models;
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
    public class ArrivalRepository : BaseRepository
    {
        public ArrivalRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task<Arrival> GetById(int id)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = @"SELECT Id, Amount, Date, Price FROM Arrival WHERE Id = @id";
                    com.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (var reader = await com.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                            return null;
                        var res = new Arrival
                        {
                            Id = await reader.GetFieldValueAsync<int>(0),
                            Amount = await reader.GetFieldValueAsync<double>(1),
                            Date = await reader.GetFieldValueAsync<DateTime>(2),
                            Price = await reader.GetFieldValueAsync<decimal>(3)
                        };
                        return res;
                    }                
                }
            }
        }
    }
}
