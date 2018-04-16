using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        internal BaseModel()
        {
            ForeignKeys = new Dictionary<string, int>();
        }

        internal Dictionary<string, int> ForeignKeys { get; set; }
    }
}
