using Application.Common.Interfaces;
using Application.Models.Product;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Returns a list of all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Product.GetAllAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return Ok(productDtos);
        }

        /// <summary>
        /// Returns a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Authorize]
        [ActionName(nameof(GetProductById))]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdReadOnlyAsync(id);
            if (product == null)
            {
                _logger.LogInfo($"А product with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto productDto)
        {
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

        /// <summary>
        /// Updates a product fully by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productForUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProductFullyById([FromRoute] Guid id, [FromBody] ProductForUpdateDto productForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid model state for 'ProductForUpdate' object");
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

        /// <summary>
        /// Removes a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProductById([FromRoute] Guid id)
        {
            var fridge = await _unitOfWork.Product.GetByIdReadOnlyAsync(id);
            if (fridge == null)
            {
                _logger.LogError($"A product with id: {id} doesn't exist in the database");
                return NotFound();
            }

            await _unitOfWork.Product.DeleteAsync(fridge.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates a product partially by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDock"></param>
        /// <returns></returns>
        [HttpPatch("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProductPartiallyById([FromRoute] Guid id,
                                                                    [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDock)
        {
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
                _logger.LogWarn("Invalid model state for 'JsonPatchDocument<ProductForUpdateDto>' object");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(productToPatch, product);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}