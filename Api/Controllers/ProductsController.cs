using Application.Common.Interfaces;
using Application.Models.Fridge;
using Application.Models.Product;
using AutoMapper;
using Domain.Entities;
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
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogInfo($"А product with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto productDto)
        {
            if (productDto == null)
            {
                _logger.LogInfo($"The sent object is null");
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);

            await _unitOfWork.Product.CreateAsync(product);
            await _unitOfWork.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(product);

            return CreatedAtRoute(nameof(GetProductById), new { id = productToReturn.Id }, productToReturn);
        }
    }
}