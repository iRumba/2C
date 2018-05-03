using Core.Models;
using Core.Repositories;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ShopManager
    {
        SimpleIoc _iocContainer;

        public ShopManager(Configuration configuration)
        {
            _iocContainer = SimpleIoc.Instance;
            _iocContainer.AddSingleton(configuration);
            _iocContainer.AddTransient<BaseRepository<Goods>, GoodsRepository>();
            _iocContainer.AddTransient<BaseRepository<Arrival>, ArrivalRepository>();
            _iocContainer.AddTransient<BaseRepository<Order>, OrderRepository>();
            _iocContainer.AddTransient<BaseRepository<Purchaser>, PurchaserRepository>();
            _iocContainer.AddTransient<BaseRepository<Purveyor>, PurveyorRepository>();
            _iocContainer.AddTransient<BaseRepository<Worker>, WorkerRepository>();
            _iocContainer.AddTransient<BaseRepository<ArrivalDetails>, ArrivalDetailsRepository>();
            _iocContainer.AddTransient<BaseRepository<OrderDetails>, OrderDetailsRepository>();
            _iocContainer.AddTransient<GoodsRepository>();
            _iocContainer.AddTransient<ArrivalRepository>();
            _iocContainer.AddTransient<OrderRepository>();
            _iocContainer.AddTransient<PurchaserRepository>();
            _iocContainer.AddTransient<PurveyorRepository>();
            _iocContainer.AddTransient<WorkerRepository>();
            _iocContainer.AddTransient<ArrivalDetailsRepository>();
            _iocContainer.AddTransient<OrderDetailsRepository>();
            _iocContainer.AddSingleton<DbManager>();
            _iocContainer.AddSingleton<RepositoryManager>();
        }

        public RepositoryManager RepositoryManager
        {
            get
            {
                return _iocContainer.Get<RepositoryManager>();
            }
        }

        public async Task ClearDatabase()
        {
            await GetDbManager().DropDatabase();
        }

        public async Task SetupDatabase()
        {
            await GetDbManager().SetupDatabase();
        }

        public async Task CreateDbIfNotExists()
        {
            var dbManager = GetDbManager();
            var checkConnectionResult = await dbManager.CheckConnection();
            switch (checkConnectionResult)
            {
                case CheckConnectionResult.Success:
                    break;
                case CheckConnectionResult.ServerConnectionError:
                    throw new InvalidOperationException("Невозможно соединиться с сервером базы данных");
                case CheckConnectionResult.DatabaseConnectionError:
                    await dbManager.CreateDatabase();
                    await dbManager.SetupDatabase();
                    break;
            }
        }

        public async Task<List<Goods>> GetAllGoods()
        {
            var rep = _iocContainer.Get<BaseRepository<Goods>>();
            return await rep.GetAll();
        }

        public async Task<Goods> GetGoodsById(int id)
        {
            var rep = _iocContainer.Get<BaseRepository<Goods>>();
            return await rep.GetById(id);
        }

        public async Task<Goods> AddGoods(Goods goods)
        {
            var rep = _iocContainer.Get<BaseRepository<Goods>>();
            return await rep.Add(goods);
        }

        public async Task<Tuple<decimal?, int>> GetGoodsDetails(int id)
        {
            var rep = _iocContainer.Get<GoodsRepository>();
            return await rep.GetDetails(id);
        }

        public async Task<List<Tuple<int, decimal?, int>>> GetGoodsDetails(IEnumerable<int> ids)
        {
            var rep = _iocContainer.Get<GoodsRepository>();
            return await rep.GetDetails(ids);
        }

        public async Task<Arrival> NewArrival(Arrival arrival)
        {
            var rep = _iocContainer.Get<BaseRepository<Arrival>>();
            return await rep.Add(arrival);
        }



        DbManager GetDbManager()
        {
            return _iocContainer.Get<DbManager>();
        }
    }
}
