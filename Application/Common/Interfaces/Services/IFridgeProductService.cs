using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IFridgeProductService
    {
        Task<FridgeProductDto> GetFridgeProductByIdAsync(Guid id);
        Task<List<FridgeProductDto>> GetProductsByFridgeIdAsync(Guid fridgeId);
        Task<List<FridgeProductDto>> CreateAsync(List<FridgeProductForCreationDto> fridgeProductForCreationDtos);
        Task DeleteFridgeProductByIdAsync(Guid id);
        Task<List<FridgeProductForCreationDto>> AddProductWhereEmptyAsync();
        Task UpdateFridgeProductAsync(FridgeProductForUpdateDto fridgeProductForUpdateDto);
    }
}