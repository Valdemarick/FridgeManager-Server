using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeProductRepository : GenericRepository<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public async Task<List<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId) =>
            await appContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId.Equals(fridgeId))
            .Include(fp => fp.Product)
            .AsNoTracking()
            .ToListAsync();

        public async Task<FridgeProduct> GetFridgeProductByIdsAsync(Guid fridgeId, Guid productId) =>
            await appContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId.Equals(fridgeId) && fp.ProductId.Equals(productId))
            .Include(fp => fp.Product)
            .AsNoTracking() 
            .SingleOrDefaultAsync();

        public async Task DeleteByIdsAsync(Guid fridgeId, Guid productId)
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

        public async Task<List<FridgeProduct>> FindRecordWhereProductQuantityIsZero()
        {
            var parameteres = new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "FridgeId",
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    Direction = System.Data.ParameterDirection.Output
                },
                new SqlParameter
                {
                    ParameterName = "ProductId",
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    Direction = System.Data.ParameterDirection.Output
                },
                new SqlParameter
                {
                    ParameterName = "ProductCount",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                },
            };

            var record = await appContext.FridgeProducts
                .FromSqlRaw("FindEmptyProducts @FridgeId OUT, @ProductId OUT, @ProductCount OUT", parameteres)
                .ToListAsync();

            return record;
        }
    }
}