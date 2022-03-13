using Application.Common.Interfaces.Services;
using Application.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Returns a list of all products
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
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
            var product = await _productService.GetProductbyIdAsync(id);
            return Ok(product);
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
                return UnprocessableEntity(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Removes a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProductById([FromRoute] Guid id)
        {
            await _productService.DeleteProductByIdAsync(id);
            return NoContent();
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
                return UnprocessableEntity(ModelState);
            }

            if (id != productForUpdateDto.Id)
            {
                return BadRequest("Incorrect id");
            }

            await _productService.UpdateProductAsync(productForUpdateDto);
            return NoContent();
        }
        
        //unnecessary
        /// <summary>
        /// Updates a product partially by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDock"></param>
        /// <returns></returns>
        //[HttpPatch("{id}"), Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> UpdateProductPartiallyById([FromRoute] Guid id,
        //                                                            [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDock)
        //{
        //    var product = await _unitOfWork.Product.GetByIdAsync(id);
        //    if (product == null)
        //    {
        //        _logger.LogInfo($"A product with id: {id} doesn't exist in the database");
        //        return NotFound();
        //    }

        //    var productToPatch = _mapper.Map<ProductForUpdateDto>(product);

        //    patchDock.ApplyTo(productToPatch);

        //    TryValidateModel(productToPatch);

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogWarn("Invalid model state for 'JsonPatchDocument<ProductForUpdateDto>' object");
        //        return UnprocessableEntity(ModelState);
        //    }

        //    _mapper.Map(productToPatch, product);

        //    await _unitOfWork.SaveAsync();

        //    return NoContent();
        //}
    }
}