using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Services;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class FridgeService : IFridgeService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public FridgeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<FridgeDto>> GetAllFridgesAsync() =>
            _mapper.Map<List<FridgeDto>>(await _unitOfWork.Fridge.GetAllAsync());

        public async Task<FridgeDto> GetFridgeByIdAsync(Guid id) =>
            _mapper.Map<FridgeDto>(await _unitOfWork.Fridge.GetByIdReadOnlyAsync(id));

        public async Task<FridgeDto> CreateFridgeAsync(FridgeForCreationDto fridgeForCreationDto)
        {
            if (fridgeForCreationDto == null)
            {
                throw new ArgumentNullException(nameof(FridgeForCreationDto));
            }

            var fridge = _mapper.Map<Fridge>(fridgeForCreationDto);
            await _unitOfWork.Fridge.CreateAsync(fridge);

            return _mapper.Map<FridgeDto>(fridge);
        }

        public async Task DeleteFridgeByIdAsync(Guid id) =>
            await _unitOfWork.Fridge.DeleteAsync(id);

        public async Task UpdateFridgeAsync(FridgeForUpdateDto fridgeForUpdateDto)
        {
            if (fridgeForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(FridgeForUpdateDto));
            }

            var fridge = _mapper.Map<Fridge>(fridgeForUpdateDto);
            await _unitOfWork.Fridge.UpdateAsync(fridge);
        }
    }
}