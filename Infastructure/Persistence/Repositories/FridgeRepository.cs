using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool tracking) =>
            await FindAll(tracking)
            .Include(f => f.FridgeModel)
            .OrderBy(f => f.Id)
            .ToListAsync();

        public async Task<Fridge> GetFridgeByIdAsync(Guid id, bool tracking) =>
            await FindByCondition(f => f.Id.Equals(id), tracking)
            .Include(f => f.FridgeModel)
            .SingleOrDefaultAsync();
    }
}