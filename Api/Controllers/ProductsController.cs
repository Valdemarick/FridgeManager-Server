using Application.Common.Interfaces;
using Application.Models.Product;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
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
        [ActionName(nameof(GetProductById))]
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
                _logger.LogError($"The sent object is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid model state for 'ProductForUpdate' object");
                return UnprocessableEntity(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);

            await _unitOfWork.Product.CreateAsync(product);
            await _unitOfWork.SaveAsync();

            var productToReturn = _mapper.Map<ProductDto>(product);

            return CreatedAtAction(nameof(GetProductById), new { id = productToReturn.Id }, productToReturn);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProductFullyById([FromRoute] Guid id, [FromBody] ProductForUpdateDto productForUpdateDto)
        {
            if (productForUpdateDto == null)
            {
                _logger.LogError("The object sent from client is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for 'ProductForUpdate' object");
                return UnprocessableEntity(ModelState);
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
                _logger.LogError($"A product with id: {id} doesn't exist in the database");
                return BadRequest();
            }

            await _unitOfWork.Product.DeleteAsync(fridge.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProductPartiallyById([FromRoute] Guid id,
                                                                    [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDock)
        {
            if (patchDock == null)
            {
                _logger.LogError("The sent object is null");
                return BadRequest();
            }

            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogInfo($"A product with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var productToPatch = _mapper.Map<ProductForUpdateDto>(product);

            patchDock.ApplyTo(productToPatch);

            TryValidateModel(productToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for 'JsonPatchDocument<ProductForUpdateDto>' object");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(productToPatch, product);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}