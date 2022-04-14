using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IFridgeProductRepository : IGenericRepository<FridgeProduct>
    {
        Task<List<FridgeProduct>> GetFridgeProductByFridgeIdAsync(Guid fridgeId);
        Task<FridgeProduct> GetFridgeProductByIdAsync(Guid id);
        Task<List<FridgeProduct>> FindRecordsWhereProductQuantityIsZeroAsync();
    }
}