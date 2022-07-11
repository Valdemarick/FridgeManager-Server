#region namespaces
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
#endregion

namespace Tests.Tests.Services
{
    public class FridgeModelServiceTests
    {
        #region fields
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FridgeModelService _fridgeModelService;
        #endregion

        #region ctor
        public FridgeModelServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeModelProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _fridgeModelService = new FridgeModelService(_fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetFridgeModelsAsync_ReturnsList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeModel.GetAllAsync()).Returns(Task.FromResult(new List<FridgeModel>()
            {
                new FridgeModel(){ Id =  Guid.NewGuid(), Name = "LG", ProductionYear = 1943 },
                new FridgeModel() { Id = Guid.NewGuid(), Name = "Atlant", ProductionYear = null }
            }));

            //Act
            var models = await _fridgeModelService.GetAllModelsAsync();

            //Assert
            Assert.NotNull(models);
            Assert.IsType<List<FridgeModelDto>>(models);

            Assert.Equal(2, models.Count());
            Assert.Equal("LG", models[0].Name);
        }
        #endregion
    }
}