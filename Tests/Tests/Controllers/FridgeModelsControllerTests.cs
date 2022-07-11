using Api.Controllers;
using Application.Models.FridgeModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;

namespace Tests.Tests.Controllers
{
    public class FridgeModelsControllerTests
    {
        private readonly FridgeModelsController _controller;
        private readonly FakeFridgeModelService _fakeService = new();

        public FridgeModelsControllerTests()
        {
            _controller = new FridgeModelsController(_fakeService.Service);
        }

        [Fact]
        public async Task GetModelsAsync_ReturnsOkObjectResult_WithList()
        {
            //Arrange
            _fakeService.Mock.Setup(fk => fk.GetAllModelsAsync()).Returns(Task.FromResult(new List<FridgeModelDto>()
            {
                new FridgeModelDto(){ Id =  Guid.NewGuid(), Name = "LG", ProductionYear = 1943 },
                new FridgeModelDto() { Id = Guid.NewGuid(), Name = "Atlant", ProductionYear = null }
            }));

            //Act
            var response = await _controller.GetModelsAsync();

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var models = result.Value as List<FridgeModelDto>;

            Assert.NotNull(models);
            Assert.IsType<List<FridgeModelDto>>(models);

            Assert.Equal(2, models.Count());
            Assert.Equal("LG", models[0].Name);
            Assert.Null(models[1].ProductionYear);
        }
    }
}