using Application.Models.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> CreateProductAsync(ProductForCreationDto productForCreationDto);
        Task DeleteProductByIdAsync(Guid id);
        Task UpdateProductAsync(ProductForUpdateDto productForUpdateDto);
    }
}