using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Table("Goods")]
    /// <summary>
    /// Товар
    /// </summary>
    public class Goods : BaseModel
    {
        public string Name { get; set; }
        public double Markup { get; set; }
        public string Image { get; set; }
    }
}
