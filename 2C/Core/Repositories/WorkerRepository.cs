using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    internal class WorkerRepository : BaseRepository<Worker>
    {
        internal WorkerRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
