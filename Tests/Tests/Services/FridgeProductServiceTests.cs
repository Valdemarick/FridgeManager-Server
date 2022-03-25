#region namespaces
using Application.Common.Mappings;
using Application.Models.Fridge;
using Application.Models.FridgeProduct;
using AutoMapper;
using Domain.Entities;
using Infastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;
#endregion

namespace Tests.Tests.Services
{
    public class FridgeProductServiceTests
    {
        #region fields
        private readonly FridgeProductService _service;
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FakeMapper _fakeMapper = new();
        #endregion

        #region ctor
        public FridgeProductServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _service = new FridgeProductService(_fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetFridgeProductByIdAsync_IncorrectGuidPasses_ReturnNull()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByIdAsync(id))
                .Returns(Task.FromResult(new FridgeProduct() { Id = id }));

            //Act
            var result = await _service.GetFridgeProductByIdAsync(Guid.NewGuid());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFridgeProductByIdAsync_CorrectGuidPasses_ReturnsData()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByIdAsync(id))
                .Returns(Task.FromResult(new FridgeProduct() { Id = id }));

            //Act
            var result = await _service.GetFridgeProductByIdAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<FridgeProductDto>(result);

            //Assert.Equal(fridgeId, result.FridgeId);
            //Assert.Equal(productId, result.ProductId);
        }

        [Fact]
        public async Task GetProductsByFridgeIdAsync_IncorrectGuidPasses_ReturnsListOfProducts()
        {
            Guid fridgeId = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId))
                .Returns(Task.FromResult(new List<FridgeProduct>()
                {
                    new FridgeProduct() { FridgeId = fridgeId, ProductQuantity = 3 },
                    new FridgeProduct() { FridgeId = fridgeId, ProductQuantity = 2 }
                }));

            //Act
            var result = await _service.GetProductsByFridgeIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductsByFridgeIdAsync_CorrectGuidPasses_ReturnsListOfProducts()
        {
            //Arrange
            Guid fridgeId = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.GetFridgeProductByFridgeIdAsync(fridgeId))
                .Returns(Task.FromResult(new List<FridgeProduct>()
                {
                    new FridgeProduct() { FridgeId = fridgeId, ProductQuantity = 3},
                    new FridgeProduct() { FridgeId = fridgeId, ProductQuantity = 2 }
                }));

            //Act
            var result = await _service.GetProductsByFridgeIdAsync(fridgeId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<FridgeProductDto>>(result);

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].ProductCount);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task CreateAsync_NullPasses_ThrowsArgumentNullException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_NotNullPasses_ReturnsList()
        {
            //Arrange
            var fridgeProductsForCreate = new List<FridgeProductForCreationDto>()
            {
                new FridgeProductForCreationDto()
                { 
                    FridgeId = Guid.NewGuid(),
                    ProductQuantity = 1,
                    ProductId = Guid.NewGuid()
                },
                new FridgeProductForCreationDto()
                {
                    FridgeId = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductQuantity = 2
                }
            };

            var fridgeProducts = _fakeMapper.Mapper.Map<List<FridgeProduct>>(fridgeProductsForCreate);

            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.CreateAsync(It.IsAny<FridgeProduct>()))
                .Returns(Task.FromResult(_fakeMapper.Mapper.Map<FridgeProduct>(fridgeProducts[0])));

            //Act
            var result = await _service.CreateAsync(fridgeProductsForCreate);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<FridgeProductDto>>(result);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].ProductCount);
        }
        #endregion

        #region add_product_where_empty
        [Fact]
        public async Task AddProductWhereEmpty_NoOneRecordFounds_ReturnsEmptyList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.FindRecorsdWhereProductQuantityIsZeroAsync()).ReturnsAsync(new List<FridgeProduct>());

            //Act
            var result = await _service.AddProductWhereEmptyAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddProductWhereEmpty_ReturnsList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.FridgeProduct.FindRecorsdWhereProductQuantityIsZeroAsync()).ReturnsAsync(new List<FridgeProduct>()
            {
                new FridgeProduct(),
                new FridgeProduct()
            });

            //Act
            var result = await _service.AddProductWhereEmptyAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.Equal(2, result.Count);
        }
        #endregion
    }
}