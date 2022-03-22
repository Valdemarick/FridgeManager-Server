#region namespaces
using Application.Common.Mappings;
using Application.Models.Product;
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
    public class ProductServiceTest
    {
        #region fields
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly ProductService _service;
        #endregion

        #region ctor
        public ProductServiceTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _service = new ProductService(_fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetAllProductsAsync_ReturnList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetAllAsync()).Returns(Task.FromResult(new List<Product>()
            {
                new Product() { Name = "Bottle of milk", Quantity = 1 },
                new Product() { Name = "Candy", Quantity = 2 }
            }));

            //Act 
            var products = await _service.GetAllProductsAsync();

            //Assert
            Assert.NotNull(products);
            Assert.IsType<List<ProductDto>>(products);

            Assert.Equal(2, products.Count);
            Assert.Equal("Candy", products[1].Name);
            Assert.Equal(1, products[0].Quantity);
        }

        [Fact]
        public async Task GetProductbyIdAsync_InvalidGuidPasses_ReturnsNull()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Product() { Name = "Bottle of beer" }));

            //Act
            var product = await _service.GetProductByIdAsync(Guid.NewGuid());

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task GetProductbyIdAsync_ValidGuidPasses_ReturnsProduct()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdReadOnlyAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new Product()
            {
                Id = id,
                Name = "Bootle of beer",
                Quantity = 9
            }));
            //Act
            var product = await _service.GetProductByIdAsync(id);

            _fakeUnitOfWork.Mock.Verify(uow => uow.Product.GetByIdReadOnlyAsync(It.IsAny<Guid>()), Times.Once);
            //Assert 
            Assert.NotNull(product);
            Assert.IsType<ProductDto>(product);

            Assert.Equal(id, product.Id);
            Assert.Equal(9, product.Quantity);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task CreateProductAsync_NullPasses_ThrowsArgumentNullException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.CreateProductAsync(null));   
        }

        [Fact]
        public async Task CreateProductAsync_NotNullPasses_ReturnsCreatedObject()
        {
            //Arrange
            var productForCreation = new ProductForCreationDto()
            {
                Name = "Egg",
                Quantity = 2
            };

            Product returnedProduct = null;

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.CreateAsync(It.IsAny<Product>()))
                .Callback<Product>(p => returnedProduct = p)
                .ReturnsAsync(_fakeMapper.Mapper.Map<Product>(productForCreation));

            //Act
            var createdProduct = await _service.CreateProductAsync(productForCreation);

            _fakeUnitOfWork.Mock.Verify(uow => uow.Product.CreateAsync(It.IsAny<Product>()), Times.Once);
            //Assert
            Assert.NotNull(createdProduct);
            Assert.IsType<ProductDto>(createdProduct);

            Assert.Equal(createdProduct.Id, returnedProduct.Id);
            Assert.Equal(productForCreation.Name, createdProduct.Name);
        }
        #endregion

        #region delete_tests
        [Fact]
        public async Task DeleteProductByIdAsync_ValidGuidPasses_PorductWillBeRemoved()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var existedProduct = new Product()
            {
                Id = id,
                Name = "Candy"
            };

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.DeleteAsync(It.IsAny<Guid>()));

            //Act
            await _service.DeleteProductByIdAsync(id);

            //Assert
            _fakeUnitOfWork.Mock.Verify(uow => uow.Product.DeleteAsync(id));
        }
        #endregion
    }
}