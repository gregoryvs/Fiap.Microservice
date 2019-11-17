using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Productions.Models;
using GeekBurger.Productions.Service;

namespace GeekBurger.Productions.Repository
{
    public class ProductionRepository : IProductionRepository
    {
        private ProductionDbContext _dbContext;
        private IProductionAreaChangedService _productionAreaChangedService;
        public ProductionRepository(ProductionDbContext context, IProductionAreaChangedService productionAreaChangedService )
        {
            _dbContext = context;
            _productionAreaChangedService = productionAreaChangedService;
        }

        public bool Add(Production production)
        {
            production.ProductionId = Guid.NewGuid();
            _dbContext.Production.Add(production);
            return true;
        }

        public void Delete(Production production)
        {
            throw new NotImplementedException();
        }

        public Production GetProductById(Guid productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Production> GetProductsByStoreName(string storeName)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _productionAreaChangedService
                .AddToMessageList(_dbContext.ChangeTracker.Entries<Production>());

            _dbContext.SaveChanges();

            _productionAreaChangedService.SendMessagesAsync();
        }

        public bool Update(Production production)
        {
            throw new NotImplementedException();
        }
    }
}
