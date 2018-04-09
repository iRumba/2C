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
    public class ArrivalRepository : BaseRepository<Arrival>
    {
        public ArrivalRepository(string connectionString) : base(connectionString)
        {

        }

        //protected override IEnumerable<string> GetFields()
        //{
        //    return base.GetFields().Concat(new string[] { "Amount", "Date", "Price" });
        //}
    }
}
