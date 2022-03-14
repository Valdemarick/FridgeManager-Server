using Application.Common.Exceptions;
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

        public async Task<FridgeProductDto> GetFridgeProductByIdsAsync(Guid fridgeId, Guid productId)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, productId);
            return _mapper.Map<FridgeProductDto>(fridgeProduct);
        }

        public async Task<IEnumerable<FridgeProductDto>> GetProductsByFridgeIdAsync(Guid fridgeId)
        {
            var fridgeProducts = await _unitOfWork.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId);
            return _mapper.Map<IEnumerable<FridgeProductDto>>(fridgeProducts);
        }

        public async Task<IEnumerable<FridgeProductDto>> CreateAsync(IEnumerable<FridgeProductForCreationDto> fridgeProductForCreationDtos)
            {
            if (fridgeProductForCreationDtos == null)
            {
                throw new ArgumentNullException(nameof(FridgeProductForCreationDto));
            }

            var createdFridgeProductRecords = new List<FridgeProductDto>();
            foreach (var fridgeProduct in fridgeProductForCreationDtos)
            {
                var fridgeProductForCreate = _mapper.Map<FridgeProduct>(fridgeProduct);
                var createdFridgeProduct = await _unitOfWork.FridgeProduct.CreateAsync(fridgeProductForCreate);

                createdFridgeProductRecords.Add(_mapper.Map<FridgeProductDto>(createdFridgeProduct));
            }

            return createdFridgeProductRecords;
        }

        public async Task DeleteFridgeProductByIdsAsync(Guid fridgeId, Guid productId) =>
            await _unitOfWork.FridgeProduct.DeleteByIdsAsync(fridgeId, productId);

        public async Task AddProductWhereEmpty()
        {
            var records = await _unitOfWork.FridgeProduct.FindRecorsdWhereProductQuantityIsZero();
            if (records == null)
            {
                throw new NotFoundException($"No one record was found");
            }

            foreach(var record in records)
            {
                await DeleteFridgeProductByIdsAsync(record.FridgeId, record.ProductId);
            }

            var fridgeProductsForCreation = _mapper.Map<List<FridgeProductForCreationDto>>(records);
            await CreateAsync(fridgeProductsForCreation);
        }
    }
}