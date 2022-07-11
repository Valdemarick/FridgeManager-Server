using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeModelRepository : GenericRepository<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }
    }
}
