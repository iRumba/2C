using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order : BaseModel
    {
        public DateTime? DepartureDate { get; set; }
        public Purchaser Purchaser { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string DeliveryMethod { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public Worker Worker { get; set; }
    }
}
