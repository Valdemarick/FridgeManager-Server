using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        DbSet<Fridge> Fridges { get; set; }
        DbSet<FridgeModel> FridgeModels { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<FridgeProduct> FridgeProducts { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}