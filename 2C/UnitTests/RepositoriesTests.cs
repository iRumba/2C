using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class RepositoriesTests<TModel> where TModel : BaseModel
    {
        //protected BaseRepository<TModel> Repository
        //{
        //    get
        //    {
        //        //var repMan = new RepositoryManager(Configuration.CONNECTION_STRING);
        //        var res = repMan.GetRepository<TModel>();
        //        return res;
        //    }
        //}
        public RepositoriesTests()
        {

        }
    }
}
