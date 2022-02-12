using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IFridgeProductRepository : IGenericRepository<FridgeProduct>
    {
        Task<IEnumerable<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId);
        Task<FridgeProduct> GetFridgeProductById(Guid fridgeId, Guid productId);
        Task DeleteByCompositeKey(Guid fridgeId, Guid productId);
    }
}