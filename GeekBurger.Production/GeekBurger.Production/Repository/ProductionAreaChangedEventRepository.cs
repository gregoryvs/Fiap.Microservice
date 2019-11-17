using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Productions.Models;

namespace GeekBurger.Productions.Repository
{
    public class ProductionChangedEventRepository : IProductionAreaChangedEventRepository
    {
        private readonly ProductionDbContext _dbContext;

        public ProductionChangedEventRepository(ProductionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(ProductionAreaChangedEvent productionAreaChangedEvent)
        {
            throw new NotImplementedException();
        }

        public ProductionAreaChangedEvent Get(Guid eventId)
        {
            return _dbContext.ProductionAreaChangedEvents
                .FirstOrDefault(production => production.EventId == eventId);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public bool Update(ProductionAreaChangedEvent productChangedEvent)
        {
            throw new NotImplementedException();
        }
    }
}
