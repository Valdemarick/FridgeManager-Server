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

namespace Tests.Tests
{
    public class FridgesControllerTests
    {
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeLoggerManager _fakeLogger = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FridgesController _controller;

        public FridgesControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _controller = new FridgesController(_fakeLogger.LoggerManager, _fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }

        [Fact]
        public async Task GetFridgesAsync_Returns_Ok_WithData()
        {
            //Arange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetAllAsync()).Returns(Task.FromResult(GetFridges()));

            //Act
            var response = await _controller.GetFridges();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);

            Assert.NotNull(okResult.Value);
            Assert.IsType<List<FridgeDto>>(okResult.Value);

            var fridges = okResult.Value as List<FridgeDto>;

            Assert.NotNull(fridges);
            Assert.Equal(GetFridges().Count, fridges.Count);

            Assert.NotNull(fridges[0]);
            Assert.Equal("Jack", fridges[0].OwnerName);
        }

        [Fact]
        public async Task GetFridgeById_InvalidGuid_Returns_NotFoundResult()
        {
            //Arange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetByIdReadOnlyAsync(Guid.NewGuid()))
                .Returns(Task.FromResult(new Fridge()
                {
                    Id = Guid.NewGuid(),
                    OwnerName = null,
                    FridgeModelId = Guid.NewGuid()
                }));

            //Act
            var response = await _controller.GetFridgeById(Guid.NewGuid());

            //Asert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            var notFound = response as NotFoundResult;

            Assert.Equal(404, notFound.StatusCode);
        }

        [Theory]
        [InlineData("2c08aafa-01ec-4902-b984-99b5a80122a3")]
        public async Task GetFridgeById_ValidGuid_Returns_Ok_WithData(string id)
        {
            //Arange
            Guid guid = new Guid(id);

            var fridgeReturn = new Fridge()
            {
                Id = guid,
                FridgeModelId = Guid.NewGuid(),
                OwnerName = "Lol"
            };

            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetByIdReadOnlyAsync(guid)).ReturnsAsync(fridgeReturn);
            //Act
            var response = await _controller.GetFridgeById(guid);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);

            Assert.NotNull(okResult.Value);
            Assert.IsAssignableFrom<FridgeDto>(okResult.Value);

            var fridge = okResult.Value as FridgeDto;

            Assert.NotNull(fridge);
            Assert.Equal("Lol", fridge.OwnerName);
            Assert.Equal(guid, fridge.Id);
        }

        [Fact]
        public async Task CreateFridge_InvalidData_ReturnsInvalidModelState()
        {
            //Arrange
            _controller.ModelState.AddModelError("OwnerName", "Much long");

            //Act
            var response = await _controller.CreateFridge(new FridgeForCreationDto()
            {
                ModelId = Guid.NewGuid(),
                OwnerName = "fdsfdsfdaskfndasonfondsofanoasdnfodsaofnsadnofoadsofosfsdafdsfjdsfnkadsad"
            });

            //Assert 
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task CreateFridge_ValidData_Returns_CreatedAtActionResult_WithData()
        {
            Guid guid = new Guid("2c08aafa-01ec-4902-b984-99b5a80122a3");

            //Arrange
            var fridgeForCreationDto = new FridgeForCreationDto()
            {
                ModelId = guid,
                OwnerName = "Jack"
            };
            var fridge = _fakeMapper.Mapper.Map<Fridge>(fridgeForCreationDto);
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.CreateAsync(fridge)).Returns(Task.FromResult(fridgeForCreationDto));

            //Act
            var response = await _controller.CreateFridge(fridgeForCreationDto);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response);
            Assert.Equal("GetFridgeById", (response as CreatedAtActionResult).ActionName);

            var result = response as CreatedAtActionResult;

            Assert.NotNull(result.Value);
            Assert.Equal(201, result.StatusCode);

            Assert.IsType<FridgeDto>(result.Value);

            var fridgeDto = result.Value as FridgeDto;

            Assert.NotNull(fridgeDto);
            Assert.Equal(fridgeDto.OwnerName, fridgeForCreationDto.OwnerName);
        }

        //[Fact]
        //public async Task DeleteFridgeById_InvalidGuid_Returns_NotFound()
        //{
        //    //Arrange
        //    Guid guid = new Guid("2c08aafa-01ec-4902-b984-99b5a80122a3");

        //    _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.DeleteAsync(Guid.NewGuid())).Returns(Task.FromResult(new Fridge()
        //    {
        //        Id = guid,
        //        OwnerName = "null",
        //        FridgeModelId = Guid.NewGuid()
        //    }));

        //    //Act 
        //    var response = await _controller.DeleteFridgeById(Guid.NewGuid());

        //    //Assert
        //    Assert.NotNull(response);
        //    Assert.IsType<NotFoundResult>(response);
        //}

        //[Fact]
        //public async Task DeleteFridbeById_ValidGuid_Returns_NoContent()
        //{
        //    //Arrange 
        //    Guid guid = new Guid("2c08aafa-01ec-4902-b984-99b5a80122a3");

        //    _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.DeleteAsync(guid)).Returns(Task.FromResult(GetFridges()));

        //    //Act
        //    var response = await _controller.DeleteFridgeById(guid);

        //    //Assert
        //    Assert.IsType<NoContentResult>(response);
        //}

        //[Fact]
        //public async Task UpdateFridgeById_InvalidGuid_Returns_NotFound()
        //{
        //    Guid id = new Guid();
        //}

        private List<Fridge> GetFridges()
        {
            return new List<Fridge>()
            {
                new Fridge
                {
                    Id = new Guid("2c08aafa-01ec-4902-b984-99b5a80122a3"),
                    OwnerName = "Jack",
                    FridgeModelId = Guid.NewGuid(),
                },
                new Fridge
                {
                    Id = Guid.NewGuid(),
                    OwnerName = "Alaska",
                    FridgeModelId = Guid.NewGuid(),
                },
                new Fridge
                {
                    Id = Guid.NewGuid(),
                    OwnerName = "Bob",
                    FridgeModelId = Guid.NewGuid(),
                },
            };
        }
    }
}
