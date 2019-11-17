using System;

namespace GeekBurger.Productions.Models
{
    public class OrderChanged
    {
        public Guid OrderId { get; set; }
        public Guid StroreId { get; set; }
        public OrderState State { get; set; }
    }
}
