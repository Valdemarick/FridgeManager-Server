#region namespaces
using Api.Controllers;
using Application.Common.Mappings;
using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
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
    public class FridgesProductsControllerTests
    {
        #region fields
        private readonly FakeFridgeProductService _fakeService = new();
        private readonly FakeMapper _fakeMapper = new();
        private readonly FridgesProductsController _controller;
        #endregion

        #region ctor
        public FridgesProductsControllerTests()
        {
            _controller = new FridgesProductsController(_fakeService.Service);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetProductsByFridgeId_NoOneRecordFounds_ReturnNotFound()
        {
            //Arrange
            Guid fridgeId = Guid.NewGuid();

            _fakeService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(fridgeId)).Returns(Task.FromResult(new List<FridgeProductDto>()
            {
                new FridgeProductDto(),
                new FridgeProductDto()
            }));

            _fakeService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<FridgeProductDto>());

            //Act
            var response = await _controller.GetProductsByFridgeIdAsync(Guid.NewGuid());
            var result = response as NotFoundResult;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetProductsByFridgeId_ReturnsOkObjectResult_WithData()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(id)).Returns(Task.FromResult(new List<FridgeProductDto>()
            {
                new FridgeProductDto() { FridgeId = id, ProductCount = 1 },
                new FridgeProductDto() { FridgeId = id, ProductCount = 3 }
            }));

            //Act
            var response = await _controller.GetProductsByFridgeIdAsync(id);
            var result = response as OkObjectResult;
            var products = result.Value as List<FridgeProductDto>;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(products);
            Assert.IsType<List<FridgeProductDto>>(products);

            Assert.Equal(id, products[0].FridgeId);
            Assert.Equal(3, products[1].ProductCount);
        }

        [Fact]
        public async Task GetFridgeProductByIdAsync_InvalidGuidPasses_ReturnsNotFound()
        {
            //Assert
            Guid id = Guid.NewGuid();

            _fakeService.Mock.Setup(s => s.GetFridgeProductByIdAsync(id)).ReturnsAsync(new FridgeProductDto()
            {
                Id = id,
                ProductCount = 3
            });

            //Act
            var response = await _controller.GetFridgeProductByIdAsync(Guid.NewGuid());
            var result = response as NotFoundResult;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetFridgeProductByIdAsync_ValidGuidPasses_ReturnsOkObjectResult_WithData()
        {
            //Assert
            Guid id = Guid.NewGuid();

            _fakeService.Mock.Setup(s => s.GetFridgeProductByIdAsync(id)).ReturnsAsync(new FridgeProductDto()
            {
                Id = id,
                ProductCount = 3
            });

            //Act
            var response = await _controller.GetFridgeProductByIdAsync(id);
            var result = response as OkObjectResult;
            var fridgeProduct = result.Value as FridgeProductDto;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(fridgeProduct);
            Assert.IsType<FridgeProductDto>(fridgeProduct);

            Assert.Equal(3, fridgeProduct.ProductCount);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task AddProductIntoFridgeAsync_InvalidModelState_ReturnsUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("ProductId", "'ProductId' property is a required field");

            //Act
            var response = await _controller.AddProductIntoFridgeAsync(new List<FridgeProductForCreationDto>());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task AddProductIntoFridgeAsync_ValidDataPasses_ReturnsCreated()
        {
            //Act
            var response = await _controller.AddProductIntoFridgeAsync(new List<FridgeProductForCreationDto>());

            //Assert
            Assert.NotNull(response);
            Assert.Equal(201, (response as StatusCodeResult).StatusCode);
        }
        #endregion

        #region delete_tests
        [Fact]
        public async Task DeleteFridgeProductByIdsAsync_ValidGuidsPasses_ReturnsNoContent()
        {
            //Act
            var response = await _controller.DeleteFridgeProductByIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }
        #endregion

        #region stored_procedure
        [Fact]
        public async Task AddProductWhereEmptyAsync_NoOneRecordFounds_ReturnsNotFound()
        {
            //Arrange
            _fakeService.Mock.Setup(s => s.AddProductWhereEmptyAsync()).ReturnsAsync(new List<FridgeProductForCreationDto>());

            //Act
            var response = await _controller.AddProductWhereEmptyAsync();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task AddProductWhereEmptyAsync_ReturnsCreated()
        {
            //Arrange
            _fakeService.Mock.Setup(s => s.AddProductWhereEmptyAsync()).ReturnsAsync(new List<FridgeProductForCreationDto>()
            {
                new FridgeProductForCreationDto()
            });

            //Act
            var response = await _controller.AddProductWhereEmptyAsync();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<StatusCodeResult>(response);

            Assert.Equal(201, (response as StatusCodeResult).StatusCode);
        }
        #endregion
    }
}