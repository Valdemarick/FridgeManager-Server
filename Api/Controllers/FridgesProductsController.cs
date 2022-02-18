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

        [HttpPost, Authorize]
        public async Task<IActionResult> AddExistProductIntoFridge([FromBody] FridgeProductForCreationDto fridgeProductDto)
        {
            if (fridgeProductDto == null)
            {
                _logger.LogError($"The sent object is null");
                return BadRequest();
            }

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

        [HttpDelete("fridge/{fridgeId}/product/{productId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFridgeProductByIds([FromRoute] Guid fridgeId, [FromRoute] Guid productId)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, productId);
            if (fridgeProduct == null)
            {
                _logger.LogInfo($"");
                return NotFound();
            }

            await _unitOfWork.FridgeProduct.DeleteByIdsAsync(fridgeId, productId);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}