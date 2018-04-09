using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Arrivals")]
    /// <summary>
    /// Приход товара
    /// </summary>
    public class Arrival : BaseModel
    {
        public Purveyor Purveyor { get; set; }
        public DateTime Date { get; set; }
    }
}
