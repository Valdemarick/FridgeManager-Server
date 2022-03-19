using Application.Models.FridgeModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IFridgeModelService
    {
        Task<List<FridgeModelDto>> GetAllModelsAsync();
    }
}