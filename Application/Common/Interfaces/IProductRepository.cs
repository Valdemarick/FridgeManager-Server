using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool tracking);
        Task<Product> GetProductByIdAsync(Guid id, bool tracking);
        Task<IEnumerable<Product>> GetProductsByFridgeIdAsync(Guid fridgeId, bool tracking);
    }
}
