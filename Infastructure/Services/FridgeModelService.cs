using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Services;
using Application.Models.FridgeModel;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class FridgeModelService : IFridgeModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FridgeModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FridgeModelDto>> GetAllModelsAsync() =>
             _mapper.Map<IEnumerable<FridgeModelDto>>(await _unitOfWork.FridgeModel.GetAllAsync());
    }
}