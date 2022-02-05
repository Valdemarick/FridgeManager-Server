using Application.Common.Interfaces;
using Application.Models.Fridge;
using Application.Models.Product;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Product.GetAllAsync();

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return Ok(productDtos);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogInfo($"The product with id: {id} dpesn't exist in the database");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        //[Route("fridge/{fridgeId}")]
        //[HttpGet]
        //public async Task<IActionResult> GetProductsByFridgeId(Guid fridgeId)
        //{
        //    var fridge = await _unitOfWork.Fridge.GetByIdAsync(fridgeId, false);

        //    if (fridge == null)
        //    {
        //        _logger.LogInfo($"");
        //        return NotFound();
        //    }

        //    var products = await _unitOfWork.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId, false);

        //    var productDtos = _mapper.Map<List<FridgeProductDto>>(products);

        //    return Ok(productDtos);
        //}
    }
}