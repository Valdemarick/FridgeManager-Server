using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridge-models")]
    [ApiController]
    public class FridgeModelsController : ControllerBase
    {
        private readonly IFridgeModelService _fridgeModelService;

        public FridgeModelsController(IFridgeModelService fridgeModelService)
        {
            _fridgeModelService = fridgeModelService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetModelsAsync()
        {
            var models = await _fridgeModelService.GetAllModelsAsync();
            return Ok(models);
        }
    }
}