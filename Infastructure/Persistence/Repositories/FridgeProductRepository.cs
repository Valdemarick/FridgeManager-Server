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
        public FridgeProductRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public async Task<IEnumerable<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId) =>
            await appContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId.Equals(fridgeId))
            .Include(fp => fp.Product)
            .AsNoTracking()
            .ToListAsync();

        public async Task<FridgeProduct> GetFridgeProductById(Guid fridgeId, Guid productId) =>
            await appContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId.Equals(fridgeId) && fp.ProductId.Equals(productId))
            .Include(fp => fp.Product)
            .AsNoTracking() 
            .SingleOrDefaultAsync();

        public async Task DeleteByCompositeKey(Guid fridgeId, Guid productId)
        {
            var existing = await appContext.Set<FridgeProduct>().FindAsync(fridgeId, productId);

            if (existing == null)
            {
                logger.LogInfo($"The cover doesn't exist in the database");
                return;
                //return NotFound();
            }

            appContext.Set<FridgeProduct>().Remove(existing);
        }
    }
}