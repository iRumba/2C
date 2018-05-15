using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    [Table("[Order]")]
    public class Order : BaseModel
    {
        public DateTime OrderDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public Purchaser Purchaser { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string DeliveryMethod { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public Worker Worker { get; set; }
    }
}
