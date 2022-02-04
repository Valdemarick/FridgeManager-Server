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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool tracking) =>
            await FindAll(tracking)
                 .OrderBy(p => p.Name)
                 .ToListAsync();

        public async Task<Product> GetProductByIdAsync(Guid id, bool tracking) =>
            await FindByCondition(p => p.Id.Equals(id), tracking)
                 .SingleOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductsByFridgeIdAsync(Guid fridgeId, bool tracking) =>
            await FindByCondition(p => p.Fridges.Any(f => f.Id.Equals(fridgeId)), tracking)
                 .OrderBy(p => p.Name)
                 .ToListAsync();
    }
}
