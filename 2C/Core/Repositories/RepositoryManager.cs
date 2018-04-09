using Core.Adapters;
using Core.Models;
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
            return (GoodsRepository)GetRepository<Goods>();
        }

        public BaseRepository<TModel> GetRepository<TModel>() where TModel : BaseModel
        {
            var type = typeof(TModel);
            return (BaseRepository<TModel>)GetRepository(type);
        }

        object GetRepository(Type type)
        {
            if (type == typeof(Goods))
                return new GoodsRepository(_connectionString);
            else if (type == typeof(Arrival))
                return new ArrivalRepository(_connectionString);
            return null;
        }
    }
}
