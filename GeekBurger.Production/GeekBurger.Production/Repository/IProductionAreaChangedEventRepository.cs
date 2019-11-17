using GeekBurger.Productions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Repository
{
    public interface IProductionAreaChangedEventRepository
    {
        ProductionAreaChangedEvent Get(Guid eventId);
        bool Add(ProductionAreaChangedEvent productionAreaChangedEvent);
        bool Update(ProductionAreaChangedEvent productChangedEvent);
        void Save();
    }
}

