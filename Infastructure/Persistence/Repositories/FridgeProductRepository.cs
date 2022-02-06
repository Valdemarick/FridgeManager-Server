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
    public class FridgeProductRepository : GenericRepository<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(ApplicationContext context, ILoggerManager logger) : base(context, logger) { }

        public async Task<IEnumerable<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId) =>
            await _appContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId.Equals(fridgeId))
            .Include(fp => fp.Product)
            .OrderBy(p => p.Product.Name)
            .AsNoTracking()
            .ToListAsync();

    }
}