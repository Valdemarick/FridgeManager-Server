using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }

        public override async Task<List<Product>> GetAllAsync() =>
            await appContext.Set<Product>()
            .AsNoTracking()
            .ToListAsync();
    }
}