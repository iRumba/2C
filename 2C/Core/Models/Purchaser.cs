using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Покупатель (клиент)
    /// </summary>
    public class Purchaser : BaseModel
    {
        public string Name { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
    }
}
