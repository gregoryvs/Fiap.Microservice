using GeekBurger.Productions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Repository
{
    public interface IProductionRepository
    {
        Production GetProductById(Guid productId);
        bool Add(Production production);
        bool Update(Production production);
        IEnumerable<Production> GetProductsByStoreName(string storeName);
        void Delete(Production production);
        void Save();
    }
}
