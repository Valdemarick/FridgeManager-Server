using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context, ILoggerManager logger) : base(context, logger) { }

        public override async Task<IEnumerable<Product>> GetAllAsync() => 
            await _appContext.Set<Product>()
            .AsNoTracking()
            .ToListAsync();
    }
}
