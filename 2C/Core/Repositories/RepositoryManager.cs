using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class RepositoryManager
    {
        string _connectionString;

        public RepositoryManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public GoodsRepository GetGoodsRepository()
        {
            var res = new GoodsRepository(_connectionString);
            return res;
        }
    }
}
