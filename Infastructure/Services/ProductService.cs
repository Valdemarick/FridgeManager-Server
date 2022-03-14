using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Services;
using Application.Models.Product;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync() =>
            _mapper.Map<List<ProductDto>>(await _unitOfWork.Product.GetAllAsync());

        public async Task<ProductDto> GetProductbyIdAsync(Guid id) =>
            _mapper.Map<ProductDto>(await _unitOfWork.Product.GetByIdReadOnlyAsync(id));

        public async Task<ProductDto> CreateProductAsync(ProductForCreationDto productForCreationDto)
        {
            if (productForCreationDto == null)
            {
                throw new ArgumentNullException(nameof(productForCreationDto));
            }

            var product = _mapper.Map<Product>(productForCreationDto);
            var createdProduct = await _unitOfWork.Product.CreateAsync(product);

            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task DeleteProductByIdAsync(Guid id) =>
            await _unitOfWork.Product.DeleteAsync(id);

        public async Task UpdateProductAsync(ProductForUpdateDto productForUpdateDto)
        {
            if (productForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(ProductForUpdateDto));
            }

            var product = _mapper.Map<Product>(productForUpdateDto);
            await _unitOfWork.Product.UpdateAsync(product);
        }
    }
}