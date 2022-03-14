using Application.Common.Interfaces.Services;
using Application.Models.FridgeProduct;
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
        private readonly IFridgeProductService _fridgeProductService;

        public FridgesProductsController(IFridgeProductService fridgeProductService)
        {
            _fridgeProductService = fridgeProductService;
        }

        /// <summary>
        /// Returns a list of products that are in a fridge with id
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpGet("fridge/{fridgeId}/products"), Authorize]
        public async Task<IActionResult> GetProductsByFridgeId([FromRoute] Guid fridgeId)
        {
            var fridgeProducts = await _fridgeProductService.GetProductsByFridgeIdAsync(fridgeId);
            return Ok(fridgeProducts);
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
            var fridgeProduct = await _fridgeProductService.GetFridgeProductByIdsAsync(fridgeId, productId);
            return Ok(fridgeProduct);
        }

        /// <summary>
        /// Adds an existing product into a fridge
        /// </summary>
        /// <param name="fridgeProductForCreationDtos"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> AddProductIntoFridge([FromBody] IEnumerable<FridgeProductForCreationDto> fridgeProductForCreationDtos)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(fridgeProductForCreationDtos);
            }

            var createdFridgesProducts = await _fridgeProductService.CreateAsync(fridgeProductForCreationDtos);
            return StatusCode(201);
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
            await _fridgeProductService.DeleteFridgeProductByIdsAsync(fridgeId, productId);
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
            await _fridgeProductService.AddProductWhereEmpty();
            return NoContent();
        }
    }
}