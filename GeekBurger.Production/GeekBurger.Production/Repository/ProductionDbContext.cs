using GeekBurger.Productions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Repository
{
    public class ProductionDbContext : DbContext
    {
        public ProductionDbContext(DbContextOptions<ProductionDbContext> options)
           : base(options)
        {
        }

        public DbSet<Production> Production { get; set; }
        public DbSet<ProductionAreaChangedEvent> ProductionAreaChangedEvents { get; set; }
    }
}
