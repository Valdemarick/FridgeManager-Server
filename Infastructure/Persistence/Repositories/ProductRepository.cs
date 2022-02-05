using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context, ILoggerManager logger) : base(context, logger) { }

        //public async Task<IEnumerable<Product>> GetProductsByFridgeIdAsync(Guid fridgeId, bool tracking) =>
        //    await FindByCondition(p => p.Fridges.Any(f => f.Id.Equals(fridgeId)), tracking)
        //         .OrderBy(p => p.Name)
        //         .ToListAsync();
    }
}
