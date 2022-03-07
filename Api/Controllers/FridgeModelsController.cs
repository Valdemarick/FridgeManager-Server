using Application.Common.Interfaces;
using Application.Models.FridgeModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridge-models")]
    [ApiController]
    public class FridgeModelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FridgeModelsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetModelsAsync()
        {
            var models = await _unitOfWork.FridgeModel.GetAllAsync();
            var modelsDtos = _mapper.Map<List<FridgeModelDto>>(models);

            return Ok(modelsDtos);
        }
    }
}