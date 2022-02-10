using Application.Common.Interfaces;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/fridges")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public FridgesController(ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFridges()
        {
            var fridges = await _unitOfWork.Fridge.GetAllAsync();

            var fridgeDtos = _mapper.Map<List<FridgeDto>>(fridges);

            return Ok(fridgeDtos);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetFridgeById([FromRoute] Guid id)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdAsync(id);

            if (fridge == null)
            {
                _logger.LogInfo($"A fridge with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var fridgeDto = _mapper.Map<FridgeDto>(fridge);

            return Ok(fridgeDto);
        }
    }
}