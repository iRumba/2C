using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    internal class PurchaserRepository : BaseRepository<Purchaser>
    {
        internal PurchaserRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
