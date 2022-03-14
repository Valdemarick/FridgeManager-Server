using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
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
            .FirstOrDefaultAsync();

        public async Task DeleteByIdsAsync(Guid fridgeId, Guid productId)
        {
            var existing = await appContext.Set<FridgeProduct>().FindAsync(fridgeId, productId);
            if (existing == null)
            {
                throw new ArgumentException($"A record with fridgeId: {fridgeId} and productId: {productId}" +
                    $"doesn't exist in the database");
            }

            appContext.Set<FridgeProduct>().Remove(existing);
            await appContext.SaveChangesAsync();
        }

        public async Task<List<FridgeProduct>> FindRecorsdWhereProductQuantityIsZero()
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