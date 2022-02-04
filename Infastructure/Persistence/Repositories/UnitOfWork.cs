using Application.Common.Interfaces;
using Infastructure.Persistence.Contexts;

namespace Infastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _appContext;
        private IFridgeRepository _fridgeRepository;
        private IProductRepository _productRepository;
        private IFridgeModelRepository _modelRepository;
        private IFridgeProductRepository _fridgeProductRepository;

        public UnitOfWork(ApplicationContext applicationContext)
        {
            _appContext = applicationContext;
        }

        public IFridgeRepository Fridge
        {
            get => _fridgeRepository ??=
                new FridgeRepository(_appContext);
        }

        public IProductRepository Product
        {
            get => _productRepository ??=
                new ProductRepository(_appContext);
        }

        public IFridgeModelRepository FridgeModel
        {
            get => _modelRepository ??=
                new FridgeModelRepository(_appContext);
        }

        public IFridgeProductRepository FridgeProduct
        {
            get => _fridgeProductRepository ??=
                new FridgeProductRepository(_appContext);
        }

        public async void SaveAsync() => await _appContext.SaveChangesAsync();
    }
}