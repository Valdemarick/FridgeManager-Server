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
            await AppContext.Set<FridgeProduct>()
            .Where(fp => fp.FridgeId == fridgeId)
            .Include(fp => fp.Product)
            .AsNoTracking()
            .ToListAsync();

        public async Task<FridgeProduct> GetFridgeProductByIdAsync(Guid id) =>
            await AppContext.Set<FridgeProduct>()
            .Where(fp => fp.Id == id)
            .Include(fp => fp.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        public async Task<List<FridgeProduct>> FindRecorsdWhereProductQuantityIsZeroAsync()
        {
            var parameteres = new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    Direction = System.Data.ParameterDirection.Output
                },
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

            var record = await AppContext.FridgeProducts
                .FromSqlRaw("FindEmptyProducts @Id OUT, @FridgeId OUT, @ProductId OUT, @ProductCount OUT", parameteres)
                .ToListAsync();

            return record;
        }
    }
}