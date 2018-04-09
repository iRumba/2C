using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Поставщик
    /// </summary>
    public class Purveyor : BaseModel
    {
        public string Name { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
