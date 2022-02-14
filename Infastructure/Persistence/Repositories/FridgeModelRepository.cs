using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeModelRepository : GenericRepository<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(IApplicationDbContext context, ILoggerManager logger) : base(context, logger) { }
    }
}
