using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Markup { get; set; }
        public byte[] Image { get; set; }
    }
}
