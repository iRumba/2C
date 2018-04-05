using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Приход товара
    /// </summary>
    public class Arrival
    {
        public int Id { get; set; }
        public Goods Goods { get; set; }
        public Purveyor Purveyor { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
