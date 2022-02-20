using Application.Models.Fridge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public interface IFridgeService
    {
        Task<IEnumerable<FridgeDto>> GetAllAsync();
        Task<FridgeDto> GetByIdAsync(Guid id);
        Task<FridgeDto> AddAsync(FridgeForCreationDto fridgeForCreationDto);
        Task UpdateFullyAsync(FridgeForUpdateDto fridgeForUpdateDto);
        Task DeleteAsync(Guid id);
        Task SaveAsync();
    }
}