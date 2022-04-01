using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Services;
using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class FridgeProductService : IFridgeProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FridgeProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FridgeProductDto> GetFridgeProductByIdAsync(Guid id)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProductByIdAsync(id);
            return _mapper.Map<FridgeProductDto>(fridgeProduct);
        }

        public async Task<List<FridgeProductDto>> GetProductsByFridgeIdAsync(Guid fridgeId)
        {
            var fridgeProducts = await _unitOfWork.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId);
            return _mapper.Map<List<FridgeProductDto>>(fridgeProducts);
        }

        public async Task<List<FridgeProductDto>> CreateAsync(List<FridgeProductForCreationDto> fridgeProductForCreationDtos)
        {
            if (fridgeProductForCreationDtos == null)
            {
                throw new ArgumentNullException(nameof(FridgeProductForCreationDto));
            }

            var createdFridgeProductRecords = new List<FridgeProductDto>();
            foreach (var fridgeProduct in fridgeProductForCreationDtos)
            {
                var createdFridgeProduct = await _unitOfWork.FridgeProduct.CreateAsync(_mapper.Map<FridgeProduct>(fridgeProduct));
                createdFridgeProductRecords.Add(_mapper.Map<FridgeProductDto>(createdFridgeProduct));
            }

            return createdFridgeProductRecords;
        }

        public async Task DeleteFridgeProductByIdAsync(Guid id) =>
            await _unitOfWork.FridgeProduct.DeleteAsync(id);

        public async Task<List<FridgeProductForCreationDto>> AddProductWhereEmptyAsync()
        {
            var records = await _unitOfWork.FridgeProduct.FindRecorsdWhereProductQuantityIsZeroAsync();

            foreach (var record in records)
            {
                await DeleteFridgeProductByIdAsync(record.Id);
            }

            return _mapper.Map<List<FridgeProductForCreationDto>>(records);
        }

        public async Task UpdateFridgeProductAsync(FridgeProductForUpdateDto fridgeProductForUpdateDto) =>
            await _unitOfWork.FridgeProduct.UpdateAsync(_mapper.Map<FridgeProduct>(fridgeProductForUpdateDto));
    }
}