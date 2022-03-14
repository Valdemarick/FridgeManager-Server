using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IFridgeProductService
    {
        Task<FridgeProductDto> GetFridgeProductByIdsAsync(Guid fridgeId, Guid productId);
        Task<IEnumerable<FridgeProductDto>> GetProductsByFridgeIdAsync(Guid fridgeId);
        Task<IEnumerable<FridgeProductDto>> CreateAsync(IEnumerable<FridgeProductForCreationDto> fridgeProductForCreationDtos);
        Task DeleteFridgeProductByIdsAsync(Guid fridgeId, Guid productId);
        Task AddProductWhereEmpty();
    }
}