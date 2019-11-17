using GeekBurger.Productions.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Service
{
    public interface IProductionAreaChangedService : IHostedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<Production>> changes);
    }
}
