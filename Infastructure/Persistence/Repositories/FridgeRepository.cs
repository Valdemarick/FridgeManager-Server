using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeRepository : GenericRepository<Fridge>, IFridgeRepository
    {
        public FridgeRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public override async Task<IEnumerable<Fridge>> GetAllAsync() =>
            await appContext.Set<Fridge>()
            .Include(f => f.FridgeModel)
            .AsNoTracking()
            .ToListAsync();

        public override async Task<Fridge> GetByIdAsync(Guid id) =>
            await appContext.Set<Fridge>()
            .Include(f => f.FridgeModel)
            .SingleOrDefaultAsync(f => f.Id.Equals(id));

        public override async Task<Fridge> GetByIdReadOnlyAsync(Guid id) =>
             await appContext.Set<Fridge>()
            .Include(f => f.FridgeModel)
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id.Equals(id));
    }
}