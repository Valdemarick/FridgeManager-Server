using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeRepository : GenericRepository<Fridge>, IFridgeRepository
    {
        public FridgeRepository(ApplicationContext context, ILoggerManager logger) : base(context, logger) { }

        public override async Task<IEnumerable<Fridge>> GetAllAsync() =>
            await _appContext.Set<Fridge>().Include(f => f.FridgeModel).ToListAsync();

        public override async Task<Fridge> GetByIdAsync(Guid id) =>
            await _appContext.Set<Fridge>().Include(f => f.FridgeModel).SingleOrDefaultAsync();
    }
}