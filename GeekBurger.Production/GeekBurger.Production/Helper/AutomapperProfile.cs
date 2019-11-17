using AutoMapper;
using GeekBurger.Productions.Contracts;
using GeekBurger.Productions.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Productions.Helper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Production, ProductionChangedMessage>();
            CreateMap<EntityEntry<Production>, ProductionAreaChangedEvent>();
        }
    }
}
