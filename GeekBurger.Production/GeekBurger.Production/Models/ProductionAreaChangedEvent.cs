using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Models
{
    public class ProductionAreaChangedEvent
    {
        [Key]
        
        public Guid EventId { get; set; }
        public Guid ProductionId { get; set; }
        //public List<string> Restrictions { get; set; }
        public string Restrictions { get; set; }
        public bool On { get; set; }
        
        public bool MessageSent { get; set; }
    }
}
