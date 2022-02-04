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
    public class FridgeProductRepository : GenericRepository<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId, bool tracking) =>
            await FindByCondition(fp => fp.FridgeId.Equals(fridgeId), false)
            .Include(p => p.Product)
            .OrderBy(fp => fp.ProductQuantity)
            .ToListAsync();
    }
}