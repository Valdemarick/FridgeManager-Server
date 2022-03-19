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
        Task<List<FridgeProductDto>> GetProductsByFridgeIdAsync(Guid fridgeId);
        Task<List<FridgeProductDto>> CreateAsync(List<FridgeProductForCreationDto> fridgeProductForCreationDtos);
        Task DeleteFridgeProductByIdsAsync(Guid fridgeId, Guid productId);
        Task AddProductWhereEmpty();
    }
}