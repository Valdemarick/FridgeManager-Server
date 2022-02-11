using Application.Common.Interfaces;
using Application.Models.Product;
using AutoMapper;
using Domain.Entities;
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

        [HttpGet]
        [Route("{id}")]
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

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProductById([FromRoute] Guid id, [FromBody] ProductForUpdateDto productForUpdateDto)
        {
            if (productForUpdateDto == null)
            {
                _logger.LogInfo("The object sent from client is null");
                return BadRequest();
            }

            var product = await _unitOfWork.Product.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogInfo($"A product with id: {id} doesn't exist in the database");
                return NotFound();
            }

            _mapper.Map(productForUpdateDto, product);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProductById([FromRoute] Guid id)
        {
            var fridge = await _unitOfWork.Product.GetByIdAsync(id);

            if (fridge == null)
            {
                _logger.LogInfo($"A product with id: {id} doesn't exist in the database");
                return BadRequest();
            }

            await _unitOfWork.Product.DeleteAsync(fridge.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}