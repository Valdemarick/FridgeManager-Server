using Api.Controllers;
using Application.Common.Mappings;
using Application.Models.Fridge;
using Application.Models.Product;
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
    public class ProductsControllerTests
    {
        private readonly FakeMapper _fakeMapper = new();
        private readonly FakeLoggerManager _fakeLogger = new();
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _controller = new ProductsController(_fakeUnitOfWork.UnitOfWork, _fakeLogger.LoggerManager, _fakeMapper.Mapper);
        }

        [Fact]
        public async Task GetAllProducts_ReturnOk_WithData()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetAllAsync()).Returns(Task.FromResult(GetProducts()));

            //Act
            var response = await _controller.GetProducts();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.IsType<List<ProductDto>>(result.Value);

            var products = result.Value as List<ProductDto>;

            Assert.NotNull(products);
            Assert.Equal(GetProducts().Count, products.Count);

            Assert.Equal("Apple", products[0].Name);
            Assert.Equal(5, products[1].Quantity);
        }

        [Fact]
        public async Task GetProductById_InvalidGuidPasses_ReturnNotFound()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdReadOnlyAsync(Guid.NewGuid())).Returns(Task.FromResult(new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Bottle of beer",
                Quantity = 7
            }));

            //Act 
            var response = await _controller.GetProductById(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            Assert.Equal(404, (response as NotFoundResult).StatusCode);
        }

        [Fact]
        public async Task GetProductById_ValidGuidPasses_ReturnOk_WithData()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdReadOnlyAsync(id)).Returns(Task.FromResult(new Product()
            {
                Id = id,
                Name = "Apple",
                Quantity = 1
            }));

            //Act
            var response = await _controller.GetProductById(id);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var product = result.Value as ProductDto;

            Assert.NotNull(product);

            Assert.IsType<ProductDto>(product);
            Assert.Equal("Apple", product.Name);
            Assert.Equal(1, product.Quantity);
        }

        [Fact]
        public async Task CreateProduct_InvalidModelState_ReturnUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("Quantity", "Wrong input data");

            //Act
            var response = await _controller.CreateProduct(new ProductForCreationDto()
            {
                Name = "Apple",
                Quantity = 1
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);

            var result = response as UnprocessableEntityObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateProduct_ValidModelState_ReturnCreatedAtAction_WithData()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var productForCreationDto = new ProductForCreationDto()
            {
                Name = "Apple",
                Quantity = 7
            };

            var product = _fakeMapper.Mapper.Map<Product>(productForCreationDto);

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.CreateAsync(product)).Returns(Task.FromResult(productForCreationDto));

            //Act
            var response = await _controller.CreateProduct(productForCreationDto);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);

            var result = response as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetProductById", result.ActionName);

            var productDto = result.Value as ProductDto;

            Assert.NotNull(productDto);
            Assert.IsType<ProductDto>(productDto);

            Assert.Equal("Apple", productDto.Name);
        }

        [Fact]
        public async Task DeleteProductById_InvalidProductIdPasses_RetunrNotFound()
        {
            //Act
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdReadOnlyAsync(Guid.NewGuid()))
                .Returns(Task.FromResult(new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Appple",
                    Quantity = 2
                }));

            //Act
            var response = await _controller.DeleteProductById(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteProductById_ValidProductIdPasses_ReturnNoContent()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdReadOnlyAsync(id))
                .Returns(Task.FromResult(new Product()
                {
                    Id = id,
                    Name = "Apple",
                    Quantity = 3
                }));
            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.DeleteAsync(id));

            //Act
            var response = await _controller.DeleteProductById(id);

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);

        }

        [Fact]
        public async Task UpdateProductFullyById_InvalidModelState_ReturnUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("Quantity", "Incorrect value");

            //Act
            var response = await _controller.UpdateProductFullyById(Guid.NewGuid(), new ProductForUpdateDto()
            {
                Name = "Apple",
                Quantity = 12
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }
        
        [Fact]
        public async Task UpdateProductFullyById_InvalidIdPasses_ReturnNotFound()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdAsync(Guid.NewGuid())).Returns(Task.FromResult(new Product()
            {
                Id = id,
                Name = "Apple",
                Quantity = 1
            }));

            //Act
            var response = await _controller.UpdateProductFullyById(Guid.NewGuid(), new ProductForUpdateDto()
            {
                Name = "Banana",
                Quantity = 2
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task UpdateProductFullyById_ValidIdPasses_ReturnNoContent()
        {
            Guid id = Guid.NewGuid();

            var product = new Product()
            {
                Id = id,
                Name = "Apple",
                Quantity = 1
            };

            _fakeUnitOfWork.Mock.Setup(uow => uow.Product.GetByIdAsync(id)).Returns(Task.FromResult(product));

            var productForUpdateDto = new ProductForUpdateDto()
            {
                Name = "Banana",
                Quantity = 2
            };

            //Act
            var response = await _controller.UpdateProductFullyById(id, productForUpdateDto);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);

            var result = response as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

            Assert.Equal(product.Name, productForUpdateDto.Name);
        }

        private List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = new Guid("2c08aafa-01ec-4902-b984-99b5a80122a3"),
                    Name = "Apple",
                    Quantity = 2
                },
                new Product()
                {
                    Id = new Guid(),
                    Name = "Bottle of milk",
                    Quantity = 5
                },
                new Product()
                {
                    Id = new Guid(),
                    Name = "Candy",
                    Quantity = 4
                }
            };
        }
    }
}