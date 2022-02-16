using Application.Common.Interfaces;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id}")]
        [ActionName(nameof(GetFridgeById))]
        public async Task<IActionResult> GetFridgeById([FromRoute] Guid id)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdReadOnlyAsync(id);

            if (fridge == null)
            {
                _logger.LogInfo($"A fridge with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var fridgeDto = _mapper.Map<FridgeDto>(fridge);

            return Ok(fridgeDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFridge([FromBody] FridgeForCreationDto fridgeForCreationDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid model state for 'FridgeForCreation' object");
                return UnprocessableEntity(ModelState);
            }

            var fridge = _mapper.Map<Fridge>(fridgeForCreationDto);

            await _unitOfWork.Fridge.CreateAsync(fridge);
            await _unitOfWork.SaveAsync();

            var fridgeToReturn = _mapper.Map<FridgeDto>(fridge);

            return CreatedAtAction(nameof(GetFridgeById), new { fridgeToReturn.Id }, fridgeToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFridgeById([FromRoute] Guid id)
        {
            var fridge = await _unitOfWork.Fridge.GetByIdReadOnlyAsync(id);

            if (fridge == null)
            {
                _logger.LogInfo($"A fridge with id: {id} doesn't exist in the databse");
                return NotFound();
            }

            await _unitOfWork.Fridge.DeleteAsync(fridge.Id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFridgeFullyById([FromRoute] Guid id ,[FromBody] FridgeForUpdateDto fridgeForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid model state for 'FridgeForCreationDto' object");
                return UnprocessableEntity(ModelState);
            }

            var fridge = await _unitOfWork.Fridge.GetByIdAsync(id);
            if (fridge == null)
            {
                _logger.LogInfo($"A fridge with id: {id} doesn't exist in the database");
                return NotFound();
            }

            _mapper.Map(fridgeForUpdateDto, fridge);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateFridgePartiallyById([FromRoute] Guid id,
                                                                   [FromBody] JsonPatchDocument<FridgeForUpdateDto> patchDock)
        {
            if (patchDock == null)
            {
                _logger.LogError("The sent object is null");
                return BadRequest();
            }

            var fridge = await _unitOfWork.Fridge.GetByIdAsync(id);
            if (fridge == null)
            {
                _logger.LogWarn($"A fridge with id: {id} doesn't exist in the database");
                return NotFound();
            }

            var fridgeToPatch = _mapper.Map<FridgeForUpdateDto>(fridge);

            patchDock.ApplyTo(fridgeToPatch);

            TryValidateModel(fridgeToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for 'FridgeForUpdate' object");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(fridgeToPatch, fridge);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}