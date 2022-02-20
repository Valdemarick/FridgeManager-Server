using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Infastructure.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FridgeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FridgeDto>> GetAllAsync()
        {
            var fridges = await _unitOfWork.Fridge.GetAllAsync();
            return _mapper.Map<List<FridgeDto>>(fridges);
        }

        public async Task<FridgeDto> GetByIdAsync(Guid id)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdAsync(id);
            return _mapper.Map<FridgeDto>(fridge);
        }


        public async Task<FridgeDto> AddAsync(FridgeForCreationDto fridgeForCreationDto)
        {
            var fridge = _mapper.Map<Fridge>(fridgeForCreationDto);
            await _unitOfWork.Fridge.CreateAsync(fridge);
            return _mapper.Map<FridgeDto>(fridge);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Fridge.DeleteAsync(id);
        }

        public async Task UpdateFullyAsync(FridgeForUpdateDto fridgeForUpdateDto)
        {
            var fridge = _mapper.Map<Fridge>(fridgeForUpdateDto);
            await _unitOfWork.Fridge.UpdateAsync(fridge);
        }

        public async Task UpdatePartially(JsonPatchDocument<FridgeForUpdateDto> patchDock)
        {

        }

        public async Task SaveAsync() => await _unitOfWork.SaveAsync();
    }
}