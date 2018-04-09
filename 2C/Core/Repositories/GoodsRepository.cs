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

        //protected override IEnumerable<string> GetFields()
        //{
        //    return base.GetFields().Concat(new string[] { "Name", "Markup", "Image" });
        //}
    }
}
