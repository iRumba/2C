﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Детали заказа
    /// </summary>
    public class OrderDetails : BaseModel
    {
        public Goods Goods { get; set; }
        public Order Order { get; set; }
        public double Amount { get; set; }
    }
}
