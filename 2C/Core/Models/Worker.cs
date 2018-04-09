using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Работник
    /// </summary>
    public class Worker : BaseModel
    {
        public string Name { get; set; }
        public string Post { get; set; }
    }
}
