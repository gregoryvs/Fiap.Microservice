using System;
using System.Collections.Generic;

namespace GeekBurger.Productions.Contracts
{
    public class ProductionChangedMessage
    {
        public Guid ProductionId { get; set; }
        public List<string> Restrictions { get; set; }
        public bool On { get; set; }
    }
}
