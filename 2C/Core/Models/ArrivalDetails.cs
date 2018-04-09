using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ArrivalDetails : BaseModel
    {
        public Arrival Arrival { get; set; }
        public Goods Goods { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
