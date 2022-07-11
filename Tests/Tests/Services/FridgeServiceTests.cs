#region namepsaces
using Application.Common.Mappings;
using Application.Models.Fridge;
using AutoMapper;
using Domain.Entities;
using Infastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Mocks;
using Xunit;
#endregion

namespace Tests.Tests.Services
{
    public class FridgeServiceTests
    {
        #region fields
        private readonly FridgeService _service;
        private readonly FakeUnitOfWork _fakeUnitOfWork = new();
        private readonly FakeMapper _fakeMapper = new();
        #endregion

        #region ctor
        public FridgeServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FridgeProfile());
            });
            _fakeMapper.Mapper = mappingConfig.CreateMapper();

            _service = new FridgeService(_fakeUnitOfWork.UnitOfWork, _fakeMapper.Mapper);
        }
        #endregion

        #region get_tests
        [Fact]
        public async Task GetAllFridgesAsync_ReturnsList()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetAllAsync()).Returns(Task.FromResult(new List<Fridge>()
            {
                new Fridge() { Id = Guid.NewGuid(), Description = "desc", OwnerName = "Vadim", FridgeModelId = Guid.NewGuid() },
                new Fridge() { Id = Guid.NewGuid(), Description = "test", OwnerName = "Kate", FridgeModelId = Guid.NewGuid() }
            }));

            //Act 
            var fridges = await _service.GetAllFridgesAsync();

            //Assert 
            Assert.NotNull(fridges);
            Assert.IsType<List<FridgeDto>>(fridges);

            Assert.Equal(2, fridges.Count());
            Assert.Equal("Vadim", fridges[0].OwnerName);
            Assert.Equal("test", fridges[1].Description);
        }

        [Fact]
        public async Task GetFridgeByIdAsync_InvalidGuid_ReturnNull()
        {
            //Assert 
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetByIdReadOnlyAsync(id)).Returns(Task.FromResult(new Fridge()
            {
                Id = id,
                Description = "desc",
                OwnerName = "Vadim",
                FridgeModelId = Guid.NewGuid()
            }));

            //Act
            var result = await _service.GetFridgeByIdAsync(Guid.NewGuid());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFridgeByIdAsync_ValidGuid_ReturnFridge()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.GetByIdReadOnlyAsync(id)).Returns(Task.FromResult(new Fridge()
            {
                Id = id,
                OwnerName = "Vadim",
                FridgeModelId = Guid.NewGuid()
            }));

            //Act 
            var fridge = await _service.GetFridgeByIdAsync(id);

            //Assert
            Assert.NotNull(fridge);
            Assert.IsType<FridgeDto>(fridge);

            Assert.Equal(id, fridge.Id);
            Assert.Equal("Vadim", fridge?.OwnerName);
        }
        #endregion

        #region create_tests
        [Fact]
        public async Task CreateFridgeAsync_NullPasses_ThrowsArgumnetNullException()
        {
            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.CreateFridgeAsync(null));
        }

        [Fact]
        public async Task CreateFridgeAsync_NotNullPasses_ReturnsCreatedObject()
        {
            //Arrange
            var fridgeForCreation = new FridgeForCreationDto()
            {
                Description = "test",
                FridgeModelId = Guid.NewGuid(),
                OwnerName = "Vadim"
            };

            Fridge createdFridge = null;

            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.CreateAsync(_fakeMapper.Mapper.Map<Fridge>(fridgeForCreation)))
                .Callback<Fridge>(f => createdFridge = f)
                .ReturnsAsync(_fakeMapper.Mapper.Map<Fridge>(fridgeForCreation));

            //Act
            var result = await _service.CreateFridgeAsync(fridgeForCreation);

            //Assert
            _fakeUnitOfWork.Mock.Verify(f => f.Fridge.CreateAsync(It.IsAny<Fridge>()), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<FridgeDto>(result);

            Assert.Equal(createdFridge.OwnerName, fridgeForCreation.OwnerName);
            Assert.Equal(result.Description, fridgeForCreation.Description);
            Assert.Equal(createdFridge.Id, result.Id);
        }
        #endregion

        #region delete_tests
        [Fact]
        public async Task DeleteFridgeByIdAsync_ValidGuidPasses_DeleteFridge()
        {
            Guid id = Guid.NewGuid();

            var fridge = new Fridge()
            {
                Id = id,
                OwnerName = "Vadim",
                Description = "test"
            };

            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.DeleteAsync(id));

            //Act
            await _service.DeleteFridgeByIdAsync(id);

            //Assert
            _fakeUnitOfWork.Mock.Verify(uow => uow.Fridge.DeleteAsync(id));
        }
        #endregion

        #region update_tests
        [Fact]
        public async Task UpdateFridgeAsync_NullPasses_ThrowsArgumentNullException()
        {
            //Arrange
            _fakeUnitOfWork.Mock.Setup(uow => uow.Fridge.UpdateAsync(It.IsAny<Fridge>()));

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateFridgeAsync(null));
        }
        #endregion
    }
}