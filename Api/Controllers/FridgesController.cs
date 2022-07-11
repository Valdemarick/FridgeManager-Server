using Application.Common.Interfaces.Services;
using Application.Models.Fridge;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly IFridgeService _fridgeService;

        public FridgesController(IFridgeService fridgeService)
        {
            _fridgeService = fridgeService;
        }

        /// <summary>
        /// Returns a list of all fridges
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetFridgesAsync()
        {
            var fridges = await _fridgeService.GetAllFridgesAsync();
            return Ok(fridges);
        }

        /// <summary>
        /// Returns a fridge by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Authorize]
        [ActionName(nameof(GetFridgeByIdAsync))]
        public async Task<IActionResult> GetFridgeByIdAsync([FromRoute] Guid id)
        {
            var fridge = await _fridgeService.GetFridgeByIdAsync(id);
            if (fridge == null)
            {
                return NotFound();
            }

            return Ok(fridge);
        }

        /// <summary>
        /// Creates a new fridge
        /// </summary>
        /// <param name="fridgeForCreationDto"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async Task<IActionResult> CreateFridgeAsync([FromBody] FridgeForCreationDto fridgeForCreationDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var createdFridge = await _fridgeService.CreateFridgeAsync(fridgeForCreationDto);
            return CreatedAtAction(nameof(GetFridgeByIdAsync), new { id = createdFridge.Id }, createdFridge);
        }

        /// <summary>
        /// Removes a fridge by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFridgeByIdAsync([FromRoute] Guid id)
        {
            await _fridgeService.DeleteFridgeByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Update a fridge 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fridgeForUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateFridgeAsync([FromRoute] Guid id, [FromBody] FridgeForUpdateDto fridgeForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (id != fridgeForUpdateDto.Id)
            {
                return BadRequest("Incorrect id");
            }

            await _fridgeService.UpdateFridgeAsync(fridgeForUpdateDto);
            return NoContent();
        }
    }
}