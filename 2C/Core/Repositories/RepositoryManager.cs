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
        public RepositoryManager()
        {

        }

        public BaseRepository<TModel> GetRepository<TModel>() where TModel : BaseModel
        {
            return SimpleIoc.Instance.Get<BaseRepository<TModel>>();
        }

        //object GetRepository(Type type)
        //{
        //    if (type == typeof(Goods))
        //        return new GoodsRepository(_connectionString);
        //    else if (type == typeof(Arrival))
        //        return new ArrivalRepository(_connectionString);
        //    else if (type == typeof(ArrivalDetails))
        //        return new ArrivalDetailsRepository(_connectionString);
        //    else if (type == typeof(Purveyor))
        //        return new PurveyorRepository(_connectionString);
        //    else if (type == typeof(OrderDetails))
        //        return new OrderDetailsRepository(_connectionString);
        //    else if (type == typeof(Worker))
        //        return new WorkerRepository(_connectionString);
        //    else if (type == typeof(Purchaser))
        //        return new PurchaserRepository(_connectionString);
        //    else if (type == typeof(Order))
        //        return new OrderRepository(_connectionString);
        //    return null;
        //}
    }
}
