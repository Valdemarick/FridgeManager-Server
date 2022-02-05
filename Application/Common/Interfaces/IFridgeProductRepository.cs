using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IFridgeProductRepository : IGenericRepository<FridgeProduct>
    {
        //Task<IEnumerable<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid firdgeId, bool tracking);
    }
}