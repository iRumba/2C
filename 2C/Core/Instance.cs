using Core.Repositories;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Instance
    {
        Configuration _configuration;
        RepositoryManager _repositoryManager;

        public Instance(Configuration configuration)
        {
            _configuration = configuration;
            _repositoryManager = new RepositoryManager(configuration.ConnectionString);
        }

        public void ClearDatabase()
        {
            GetDbManager().ClearDatabase();
        }

        public void SetupDatabase()
        {
            GetDbManager().SetupDatabase();
        }

        DbManager GetDbManager()
        {
            return new DbManager(_configuration.ConnectionString);
        }
    }
}
