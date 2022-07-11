using Application.Models.Fridge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IFridgeService
    {
        Task<List<FridgeDto>> GetAllFridgesAsync();
        Task<FridgeDto> GetFridgeByIdAsync(Guid id);
        Task<FridgeDto> CreateFridgeAsync(FridgeForCreationDto fridgeForCreationDto);
        Task DeleteFridgeByIdAsync(Guid id);
        Task UpdateFridgeAsync(FridgeForUpdateDto fridgeForUpdateDto);
    }
}