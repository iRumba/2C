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
    public class GoodsRepository : BaseRepository
    {
        internal GoodsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<Goods> GetById(int id)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = @"SELECT Id, Name, Markup, Image FROM Goods WHERE Id=@id";
                    com.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var adapter = new GoodsAdapter();
                    var res = await adapter.GetModelFromDataSetAsync(await com.ToDataSetAsync());
                    return res;
                }
            }
        }

        public async Task<List<Goods>> GetByIds(IEnumerable<int> ids)
        {
            using (var con = GetConnection())
            {
                await con.OpenAsync();
                using (var com = con.CreateCommand())
                {
                    com.CommandType = CommandType.Text;
                    var sb = new StringBuilder("SELECT Id, Name, Markup, Image FROM Goods WHERE ");
                    if (!ids.Any())
                        sb.Append("0=1");
                    else
                    {
                        var counter = 0;
                        foreach(var id in ids)
                        {
                            if (counter > 0)
                                sb.Append(" OR ");
                            com.Parameters.Add($"@id{counter}", SqlDbType.Int).Value = id;
                            sb.Append($"Id=@id{counter}");
                            counter++;
                        }
                    }

                    com.CommandText = sb.ToString();
                    var adapter = new GoodsAdapter();
                    var res = await adapter.GetModelListFromDataSetAsync(await com.ToDataSetAsync());
                    return res;
                }
            }
        }
    }
}
