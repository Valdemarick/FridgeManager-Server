using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFridgeProductRepository : IGenericRepository<FridgeProduct>
    {
        Task<List<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId);
        Task<FridgeProduct> GetFridgeProductByIdsAsync(Guid fridgeId, Guid productId);
        Task DeleteByIdsAsync(Guid fridgeId, Guid productId);
        Task<List<FridgeProduct>> FindRecorsdWhereProductQuantityIsZero();
    }
}