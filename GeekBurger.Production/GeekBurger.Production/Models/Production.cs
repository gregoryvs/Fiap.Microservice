using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Productions.Models
{
    public class Production
    {
        [Key]
        public Guid ProductionId { get; set; }
        public string Restrictions { get; set; }

        //public ICollection<string> Restrictions { get; set; } 
        //      = new List<string>();
        public bool On { get; set; }
    }
}