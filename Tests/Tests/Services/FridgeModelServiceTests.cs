using Application.Common.Mappings;
using Application.Models.FridgeModel;
using AutoMapper;
using Domain.Entities;
using Infastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;

namespace Tests.Tests.Services
{
    public class FridgeModelServiceTests
    {
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FridgeModelService _fridgeModelService;

        public FridgeModelServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeModelProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _fridgeModelService = new FridgeModelService(_fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }

        [Fact]
        public async Task GetFridgeModelsAsync_ReturnsOk_WithFridgeModelList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeModel.GetAllAsync()).Returns(Task.FromResult(new List<FridgeModel>()
            {
                new FridgeModel(){ Id =  Guid.NewGuid(), Name = "LG", ProductionYear = 1943 },
                new FridgeModel() { Id = Guid.NewGuid(), Name = "Atlant", ProductionYear = null }
            }));

            //Act
            var response = await _fridgeModelService.GetAllModelsAsync();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<List<FridgeModelDto>>(response);
            Assert.Equal(2, response.Count());
            Assert.Equal("LG", response[0].Name);
        }
    }
}