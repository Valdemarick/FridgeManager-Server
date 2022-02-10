using Application.Common.Interfaces;
using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridgesproducts")]
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

        [HttpGet]
        [Route("{fridgeId}")]
        public async Task<IActionResult> GetProductsByFridgeId([FromRoute] Guid fridgeId)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdAsync(fridgeId);

            if (fridge == null)
            {
                _logger.LogInfo($"A fridge with id: {fridgeId} doesn't exist in the database");
                return NotFound();
            }

            var products = await _unitOfWork.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId);

            var productDtos = _mapper.Map<List<FridgeProductDto>>(products);

            return Ok(productDtos);
        }

        [HttpGet]
        [Route("{fridgeId}/product/{productId}")]
        public async Task<IActionResult> GetFridgeProductbyIds([FromRoute] Guid fridgeId, Guid productId)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProdcutById(fridgeId, productId);

            if (fridgeProduct == null)
            {
                _logger.LogInfo($"A cover doesn't exist in the database");
                return NotFound();
            }

            var fridgeProductDto = _mapper.Map<FridgeProductDto>(fridgeProduct);

            return Ok(fridgeProductDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddExistProductIntoFridge([FromBody] FridgeProductForCreationDto fridgeProductDto)
        {
            if (fridgeProductDto == null)
            {
                _logger.LogInfo($"The sent object is null");
                return BadRequest();
            }

            var existing = await _unitOfWork.FridgeProduct.GetFridgeProdcutById(fridgeProductDto.FridgeId, fridgeProductDto.ProductId);

            if (existing != null)
            {
                _logger.LogInfo($"A cover already exist in the database");
                return BadRequest();
            }

            var fridgeProduct = _mapper.Map<FridgeProduct>(fridgeProductDto);

            await _unitOfWork.FridgeProduct.CreateAsync(fridgeProduct);
            await _unitOfWork.SaveAsync();

            var fridgeProductToReturn = _mapper.Map<FridgeProductDto>(fridgeProduct);
            var route = $"{fridgeProductDto.FridgeId}/product/{fridgeProductToReturn.ProductId}";
            //////////////////////////
            return CreatedAtRoute(nameof(GetFridgeProductbyIds), new
            { route }, fridgeProductDto);
        }

        [HttpDelete]
        [Route("{fridgeId}/product/{productId}")]
        public async Task<IActionResult> DeleteFridgeProductByIds([FromRoute] Guid fridgeId, [FromRoute] Guid productId)
        {
            var fridgeProduct = await _unitOfWork.FridgeProduct.GetFridgeProdcutById(fridgeId, productId);

            if (fridgeProduct == null)
            {
                _logger.LogInfo($"");
                return NotFound();
            }

            await _unitOfWork.FridgeProduct.DeleteByCompositeKey(fridgeId, productId);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}