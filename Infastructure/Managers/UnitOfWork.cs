using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Infastructure.Persistence.Repositories;
using System.Threading.Tasks;

namespace Infastructure.Managers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _appContext;
        private readonly ILoggerManager _logger;

        private IFridgeRepository _fridgeRepository;
        private IProductRepository _productRepository;
        private IFridgeModelRepository _modelRepository;
        private IFridgeProductRepository _fridgeProductRepository;

        public UnitOfWork(IApplicationDbContext applicationContext, ILoggerManager logger)
        {
            _appContext = applicationContext;
            _logger = logger;
        }

        public IFridgeRepository Fridge
        {
            get => _fridgeRepository ??=
                new FridgeRepository(_appContext, _logger);
        }

        public IProductRepository Product
        {
            get => _productRepository ??=
                new ProductRepository(_appContext, _logger);
        }

        public IFridgeModelRepository FridgeModel
        {
            get => _modelRepository ??=
                new FridgeModelRepository(_appContext, _logger);
        }

        public IFridgeProductRepository FridgeProduct
        {
            get => _fridgeProductRepository ??=
                new FridgeProductRepository(_appContext, _logger);
        }

        public async Task SaveAsync() => await _appContext.SaveChangesAsync();
    }
}