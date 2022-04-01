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
        public async Task<IActionResult> GetProductsByFridgeIdAsync([FromRoute] Guid fridgeId)
        {
            var fridgeProducts = await _fridgeProductService.GetProductsByFridgeIdAsync(fridgeId);
            if (fridgeProducts.Count == 0)
            {
                return NotFound();
            }

            return Ok(fridgeProducts);
        }

        /// <summary>
        /// Returns information about a product by productId that is in a fridge with fridgeId 
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Authorize]
        [ActionName(nameof(GetFridgeProductByIdAsync))]
        public async Task<IActionResult> GetFridgeProductByIdAsync([FromRoute] Guid id)
        {
            var fridgeProduct = await _fridgeProductService.GetFridgeProductByIdAsync(id);
            if (fridgeProduct == null)
            {
                return NotFound();
            }

            return Ok(fridgeProduct);
        }

        /// <summary>
        /// Adds an existing product into a fridge
        /// </summary>
        /// <param name="fridgeProductForCreationDtos"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> AddProductsIntoFridgeAsync([FromBody] List<FridgeProductForCreationDto> fridgeProductForCreationDtos)
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
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFridgeProductByIdAsync([FromRoute] Guid id)
        {
            await _fridgeProductService.DeleteFridgeProductByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Calls the stored procedure that finds records in the 'Fridges_Products' table where 'ProductQuantity' equal zero
        /// and adds products with a 'Quantity' from 'Products' table into 'Fridges_Products'
        /// </summary>
        /// <returns></returns>
        [HttpPost("where-products-are-empty"), Authorize]
        public async Task<IActionResult> AddProductsWhereEmptyAsync()
        {
            var records = await _fridgeProductService.AddProductWhereEmptyAsync();
            if (records.Count == 0)
            {
                return NotFound();
            }

            return await AddProductsIntoFridgeAsync(records);
        }

        /// <summary>
        /// Updates a fridge product
        /// </summary>
        /// <param name="fridgeProductForUpdateDto"></param>
        /// <returns></returns>
        [HttpPut, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateFridgeProductAsync(FridgeProductForUpdateDto fridgeProductForUpdateDto)
        {
            if (!ModelState.IsValid) 
            {
                return UnprocessableEntity(fridgeProductForUpdateDto);
            }

            await _fridgeProductService.UpdateFridgeProductAsync(fridgeProductForUpdateDto);
            return NoContent();
        }
    }
}