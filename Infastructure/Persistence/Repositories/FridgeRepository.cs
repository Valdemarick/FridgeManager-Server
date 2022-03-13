using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeRepository : GenericRepository<Fridge>, IFridgeRepository
    {
        public FridgeRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public override async Task<List<Fridge>> GetAllAsync() =>
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