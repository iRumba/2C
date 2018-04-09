using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        internal BaseModel()
        {
            ForeignKeys = new Dictionary<string, int>();
        }

        internal Dictionary<string, int> ForeignKeys { get; set; }
    }
}
