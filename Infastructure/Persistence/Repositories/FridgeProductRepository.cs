using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistence.Repositories
{
    public class FridgeProductRepository : GenericRepository<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(ApplicationContext context, ILoggerManager logger) : base(context, logger) { }
    }
}