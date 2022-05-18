#region namespaces
using Api.Controllers;
using Application.Common.Mappings;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;
#endregion

namespace Tests.Tests.Controllers
{
    public class FridgesControllerTests
    {
        #region fields
        private readonly FakeFridgeService _service = new();
        private readonly FakeMapper _fakeMapper = new();
        private readonly FridgesController _controller;
        #endregion

        #region ctor
        public FridgesControllerTests()
        {
            _controller = new FridgesController(_service.Service);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetFridgesAsync_ReturnsOkObjectResult_WithData()
        {
            //Arrange
            _service.Mock.Setup(s => s.GetAllFridgesAsync()).ReturnsAsync(new List<FridgeDto>()
            {
                new FridgeDto() { Id = Guid.NewGuid(), OwnerName = "Vadim", Description = "test" },
                new FridgeDto() { Id = Guid.NewGuid(), OwnerName = "Jack", Description = null }
            });

            //Act
            var response = await _controller.GetFridgesAsync();
            var result = response as OkObjectResult;
            var fridges = result.Value as List<FridgeDto>;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(fridges);
            Assert.IsType<List<FridgeDto>>(fridges);

            Assert.Equal(2, fridges.Count);
            Assert.Equal("Vadim", fridges[0].OwnerName);
            Assert.Null(fridges[1].Description);
        }

        [Fact]
        public async Task GetFridgeByIdAsync_InvalidGuidPasses_ReturnsNotFound()
        {
            //Act
            var response = await _controller.GetFridgeByIdAsync(Guid.NewGuid());
            var result = response as NotFoundResult;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetFridgeByIdAsync_ValidGuidPasses_ReturnsOkObjectResult_WithData()
        {
            //Arrange
            Guid id = new Guid();

            _service.Mock.Setup(s => s.GetFridgeByIdAsync(id)).Returns(Task.FromResult(new FridgeDto()
            {
                Id = id,
                OwnerName = "Vadim"
            }));

            //Act
            var response = await _controller.GetFridgeByIdAsync(id);
            var result = response as OkObjectResult;
            var fridge = result.Value as FridgeDto;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(fridge);
            Assert.IsType<FridgeDto>(fridge);

            Assert.Equal(id, fridge.Id);
            Assert.Equal("Vadim", fridge.OwnerName);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task CreateFridgeAsync_InvalidModelState_ReturnsUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("FridgeModelId", "ModelId is required");

            //Act
            var response = await _controller.CreateFridgeAsync(new FridgeForCreationDto());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task CreateFridgeAsync_ValidModelState_ReturnsCreatedAtActionResult_WithData()
        {
            //Arrange
            var fridgeForCreation = new FridgeForCreationDto()
            {
                OwnerName = "Vadim",
                Description = "test"
            };

            var fridge = _fakeMapper.Mapper.Map<Fridge>(fridgeForCreation);

            _service.Mock.Setup(s => s.CreateFridgeAsync(fridgeForCreation)).ReturnsAsync(_fakeMapper.Mapper.Map<FridgeDto>(fridge));

            //Act
            var response = await _controller.CreateFridgeAsync(fridgeForCreation);
            var result = response as CreatedAtActionResult;
            var createdFridge = result?.Value as FridgeDto;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);

            Assert.NotNull(createdFridge);
            Assert.IsType<FridgeDto>(createdFridge);

            Assert.Equal("Vadim", createdFridge.OwnerName);
        }
        #endregion

        #region update_tests
        [Fact]
        public async Task UpdateFridgeAsync_InvalidModelState_ReturnsUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("FridgeModelId", "ModelId is required");

            //Act
            var response = await _controller.UpdateFridgeAsync(new Guid(), new FridgeForUpdateDto());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task UpdateFridgeAsync_DifferentGuidsPasses_ReturnsBadRequest()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var fridgeForUpdate = new FridgeForUpdateDto() { Id = new Guid() };

            //Act
            var response = await _controller.UpdateFridgeAsync(id, fridgeForUpdate);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        #endregion

        #region delete_tests
        [Fact]
        public async Task DeleteFridgeByIdAsync_ValidGuidPasses_ReturnsNoContent()
        {
            //Act
            var response = await _controller.DeleteFridgeByIdAsync(new Guid());

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }
        #endregion
    }
}