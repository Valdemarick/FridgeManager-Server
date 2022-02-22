using Api.Controllers;
using Application.Common.Mappings;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;


namespace Tests.Tests
{
    public class FridgesProductsControllerTests
    {
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeLoggerManager _fakeLogger = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FridgesProductsController _controller;

        public FridgesProductsControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _controller = new FridgesProductsController(_fakeUnitOfWork.UnitOfWork, _fakeLogger.LoggerManager, _fakeMapper.Mapper);
        }

        [Fact]
        public async Task GetProductsByFridgeId_InvalidGuidPasses_ReturnNotFound()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetByIdReadOnlyAsync(Guid.NewGuid())).Returns(Task.FromResult(new Fridge()
            {
                Id = Guid.NewGuid(),
                FridgeModelId = Guid.NewGuid(),
                OwnerName = null
            }));

            //Act
            var response = await _controller.GetProductsByFridgeId(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            var result = response as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetProductsByFridgeId_ValidGuidPasses_ReturnOk_WithData()
        {
            //Act
            Guid id = Guid.NewGuid();
            _fakeUnitOfWork.Mock.Setup(upw => upw.Fridge.GetByIdReadOnlyAsync(id)).Returns(Task.FromResult(new Fridge()
            {
                Id = id,
                FridgeModelId = Guid.NewGuid(),
            }));
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByFridgeIdAsync(id))
                .Returns(Task.FromResult(new List<FridgeProduct>()
                {
                    new FridgeProduct()
                    {
                        FridgeId = id,
                        ProductId = Guid.NewGuid(),
                        ProductQuantity = 2
                    },
                    new FridgeProduct()
                    {
                        FridgeId = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductQuantity = 1
                    }
                }));

            //Act
            var response = await _controller.GetProductsByFridgeId(id);

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var products = result.Value as List<FridgeProductDto>;

            Assert.NotNull(products);
            Assert.IsType<List<FridgeProductDto>>(products);

            Assert.Equal(2, products[0].ProductCount);
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GetFrigeProductByIds_InvalidFridgeIdPasses_ReturnNotFound()
        {
            //Arrange
            Guid productId = Guid.NewGuid();
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByIdsAsync(Guid.NewGuid(), productId))
                .Returns(Task.FromResult(new FridgeProduct()
                {
                    FridgeId = Guid.NewGuid(),
                    ProductId = productId,
                    ProductQuantity = 1
                }));

            //Act
            var response = await _controller.GetFridgeProductbyIds(Guid.NewGuid(), productId);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task GetFrigeProductByIds_InvalidProductIdPasses_ReturnNotFound()
        {
            Guid fridgeId = Guid.NewGuid();
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, Guid.NewGuid()))
                .Returns(Task.FromResult(new FridgeProduct()
                {
                    FridgeId = fridgeId,
                    ProductId = Guid.NewGuid(),
                    ProductQuantity = 1
                }));

            //Act
            var response = await _controller.GetFridgeProductbyIds(fridgeId, Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task GetFrigeProductByIds_ValidGuidsPasses_ReturnOk_WithData()
        {
            //Arrange
            Guid fridgeId = Guid.NewGuid();
            Guid productId = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByIdsAsync(fridgeId, productId))
                .Returns(Task.FromResult(new FridgeProduct()
                {
                    FridgeId = fridgeId,
                    ProductId = productId,
                    ProductQuantity = 2
                }));

            //Act
            var response = await _controller.GetFridgeProductbyIds(fridgeId, productId);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var fridgeProduct = result.Value as FridgeProductDto;

            Assert.NotNull(fridgeProduct);
            Assert.IsType<FridgeProductDto>(fridgeProduct);

            Assert.Equal(2, fridgeProduct.ProductCount);
            Assert.Equal(productId, fridgeProduct.ProductId);
        }
    }
}