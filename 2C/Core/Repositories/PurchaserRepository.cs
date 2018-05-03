using Core.Models;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    internal class PurchaserRepository : BaseRepository<Purchaser>
    {
        public PurchaserRepository(DbManager dbManager) : base(dbManager)
        {
        }
    }
}
