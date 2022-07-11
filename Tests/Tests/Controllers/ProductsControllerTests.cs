#region namespaces
using Api.Controllers;
using Application.Common.Mappings;
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
#endregion

namespace Tests.Tests.Controllers
{
    public class ProductsControllerTests
    {
        #region fields
        private readonly FakeProductService _service = new();
        private readonly FakeMapper _fakeMapper = new();
        private readonly ProductsController _controller;
        #endregion

        #region ctor
        public ProductsControllerTests()
        {
            _controller = new ProductsController(_service.Service);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetProductAsync_ReturnsOkObjectResult_WithList()
        {
            //Arrange
            _service.Mock.Setup(s => s.GetAllProductsAsync()).Returns(Task.FromResult(new List<ProductDto>()
            {
                new ProductDto() { Name = "Candy", Quantity = 2 },
                new ProductDto() { Name = "Bottle of beer", Quantity = 5 }
            }));

            //Act
            var response = await _controller.GetProductsAsync();
            var result = response as OkObjectResult;
            var products = result.Value as List<ProductDto>;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(products);
            Assert.Equal(2, products.Count);

            Assert.Equal("Candy", products[0].Name);
            Assert.Equal(5, products[1].Quantity);
        }

        [Fact]
        public async Task GetProductByIdAsync_InvalidGuidPasses_ReturnsNull()
        {
            //Assert
            Guid id = new Guid();

            _service.Mock.Setup(s => s.GetProductByIdAsync(id)).Returns(Task.FromResult(new ProductDto()
            {
                Id = Guid.NewGuid(),
                Name = "Candy"
            }));

            //Act 
            var response = await _controller.GetProductByIdAsync(Guid.NewGuid());
            var result = response as NotFoundResult;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_ValidGuidPasses_ReturnsOkObjectResult_WithData()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _service.Mock.Setup(s => s.GetProductByIdAsync(id)).Returns(Task.FromResult(new ProductDto()
            {
                Id = id,
                Name = "Candy",
                Quantity = 1
            }));

            //Act
            var response = await _controller.GetProductByIdAsync(id);
            var result = response as OkObjectResult;
            var product = result?.Value as ProductDto;

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            Assert.NotNull(product);
            Assert.IsType<ProductDto>(product);

            Assert.Equal(id, product.Id);
            Assert.Equal("Candy", product.Name);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task CreateProductAsync_InvalidModelState_ReturnsUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("Quantity", "Name is required");

            //Act
            var response = await _controller.CreateProductAsync(new ProductForCreationDto()
            {
                Quantity = 21
            });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task CreateProductAsync_ValidModelState_ReturnsCreatedAtAction_WithData()
        {
            //Arrange
            var productForCreation = new ProductForCreationDto()
            {
                Name = "Bottle of milk",
                Quantity = 3
            };
            var product = _fakeMapper.Mapper.Map<Product>(productForCreation);

            _service.Mock.Setup(s => s.CreateProductAsync(productForCreation)).ReturnsAsync(_fakeMapper.Mapper.Map<ProductDto>(product));

            //Act
            var response = await _controller.CreateProductAsync(productForCreation);
            var result = response as CreatedAtActionResult;
            var createdProduct = result.Value as ProductDto;

            //Assert 
            Assert.NotNull(response);
            Assert.IsType<CreatedAtActionResult>(response);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);

            Assert.NotNull(createdProduct);
            Assert.IsType<ProductDto>(createdProduct);

            Assert.Equal(productForCreation.Name, createdProduct.Name);
            Assert.Equal(productForCreation.Quantity, createdProduct.Quantity);
        }
        #endregion

        #region update_tests
        [Fact]
        public async Task UpdateProductByIdAsync_InvalidModelState_ReturnsUnprocessableEntity()
        {
            //Arrange
            _controller.ModelState.AddModelError("Name", "Maximum length of Name is 30 characters");

            //Act 
            var response = await _controller.UpdateProductByIdAsync(Guid.NewGuid(), new ProductForUpdateDto());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<UnprocessableEntityObjectResult>(response);
        }

        [Fact]
        public async Task UpdateProductByIdAsync_DifferentGuidsPasses_ReturnsBadRequest()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var productForUpdate = new ProductForUpdateDto()
            {
                Id = Guid.NewGuid()
            };

            //Act
            var response = await _controller.UpdateProductByIdAsync(id, productForUpdate);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task UpdateProductByIdAsync_ValidDataPasses_ReturnsNoContent()
        {
            //Arrange 
            Guid id = Guid.NewGuid();
            var productForUpdate = new ProductForUpdateDto() { Id = id };

            //Act
            var response = await _controller.UpdateProductByIdAsync(id, productForUpdate);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }
        #endregion

        #region delete_tests
        [Fact]
        public async Task DeleteProductByIdAsync_ValidGuidPasses_ReturnsNoContent()
        {
            //Act
            var response = await _controller.DeleteProductByIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }
        #endregion
    }
}