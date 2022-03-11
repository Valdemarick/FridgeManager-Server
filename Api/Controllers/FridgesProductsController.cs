using Application.Common.Interfaces;
using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridges-products")]
    [ApiController]
    public class FridgesProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public FridgesProductsController(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of products that are in a fridge with id
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpGet("fridge/{fridgeId}/products"), Authorize]
        public async Task<IActionResult> GetProductsByFridgeId([FromRoute] Guid fridgeId)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdReadOnlyAsync(fridgeId);
            if (fridge == null)
            {
                _logger.LogError($"A fridge with id: {fridgeId} doesn't exist in the database");
                return NotFound();
            }

            var products = await _unitOfWork.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId);
            var productDtos = _mapper.Map<List<FridgeProductDto>>(products);

            return Ok(productDtos);
        }

        /// <summary>
        /// Returns information about a product by productId that is in a fridge with fridgeId 
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("fridge/{fridgeId}/product/{productId}"), Authorize]
        [ActionName(nameof(GetFridgeProductbyIds))]
        public async Task<IActionResult> GetFridgeProductbyIds([FromRoute] Guid fridgeId, Guid productId)
        {
            var fridgeProducts = await _unitOfWork.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, productId);
            if (fridgeProducts == null)
            {
                _logger.LogError($"A record doesn't exist in the database");
                return NotFound();
            }

            var fridgeProductDto = _mapper.Map<FridgeProductDto>(fridgeProducts);

            return Ok(fridgeProductDto);
        }

        /// <summary>
        /// Adds an existing product into a fridge
        /// </summary>
        /// <param name="fridgeProductDto"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> AddExistProductIntoFridge([FromBody] FridgeProductForCreationDto fridgeProductDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for 'FridgeProductForCreationDto' object");
                return UnprocessableEntity(ModelState);
            }

            var existing = await _unitOfWork.FridgeProduct.GetFridgeProductByIdsAsync(fridgeProductDto.FridgeId, fridgeProductDto.ProductId);
            if (existing != null)
            {
                _logger.LogError($"A record already exist in the database");
                return BadRequest();
            }

            var fridge = await _unitOfWork.Fridge.GetByIdReadOnlyAsync(fridgeProductDto.FridgeId);
            if (fridge == null)
            {
                _logger.LogError($"A fridge with id:{fridgeProductDto.FridgeId} doesn't exist in the database");
                return NotFound();
            }

            var product = await _unitOfWork.Product.GetByIdReadOnlyAsync(fridgeProductDto.ProductId);
            if (product == null)
            {
                _logger.LogError($"A product with id:{fridgeProductDto.ProductId} doesn't exist in the database");
                return NotFound();
            }

            var fridgeProduct = _mapper.Map<FridgeProduct>(fridgeProductDto);

            await _unitOfWork.FridgeProduct.CreateAsync(fridgeProduct);
            await _unitOfWork.SaveAsync();

            var fridgeProductToReturn = _mapper.Map<FridgeProductDto>(fridgeProduct);

            return CreatedAtAction(nameof(GetFridgeProductbyIds), new
            {
                fridgeId = fridgeProductDto.FridgeId,
                productId = fridgeProductToReturn.ProductId
            }, fridgeProductToReturn);
        }

        /// <summary>
        /// Removes a product with productId from a fridge with fridgeId
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("fridge/{fridgeId}/product/{productId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFridgeProductByIds([FromRoute] Guid fridgeId, [FromRoute] Guid productId)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, productId);
            if (fridgeProduct == null)
            {
                _logger.LogInfo($"A record with fridgeID:{fridgeId} and productID:{productId} doesn't exist in the database");
                return NotFound();
            }

            await _unitOfWork.FridgeProduct.DeleteByIdsAsync(fridgeId, productId);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Calls the stored procedure that finds records in the 'Fridges_Products' table where 'ProductQuantity' equal zero
        /// and adds products with a 'Quantity' from 'Products' table into 'Fridges_Products'
        /// </summary>
        /// <returns></returns>
        [HttpPut("where-products-are-empty")]
        public async Task<IActionResult> AddProductWhereEmpty()
        {
            var records = await _unitOfWork.FridgeProduct.FindRecordWhereProductQuantityIsZero();
            if (records == null)
            {
                _logger.LogInfo("There are no one record with 'Product Quantity' equal zero");
                return NotFound();
            }

            foreach (var record in records)
            {
                await _unitOfWork.FridgeProduct.DeleteByIdsAsync(record.FridgeId, record.ProductId);
                await _unitOfWork.SaveAsync();

                await AddExistProductIntoFridge(new FridgeProductForCreationDto()
                {
                    FridgeId = record.FridgeId,
                    ProductId = record.ProductId,
                    ProductQuantity = record.ProductQuantity
                });
            }

            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}