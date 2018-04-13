using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class PurveyorRepository : BaseRepository<Purveyor>
    {
        internal PurveyorRepository(string connectionString) : base(connectionString)
        {

        }
    }
}
