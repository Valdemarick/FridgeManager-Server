using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeRepository : GenericRepository<Fridge>, IFridgeRepository
    {
        public FridgeRepository(ApplicationContext context) : base(context) { }
    }
}
