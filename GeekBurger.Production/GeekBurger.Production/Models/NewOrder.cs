using System;
using System.Collections.Generic;

namespace GeekBurger.Productions.Models
{
    public class NewOrder
    {
        public Guid OrderId { get; set; }
        public Guid StoreId { get; set; }
        public decimal Total { get; set; }
        public List<Pruduct> Products { get; set; }
        public List<Guid> ProductionsIds { get; set; }
    }

    public class Pruduct
    {
       public Guid ProductId;
    }
}
