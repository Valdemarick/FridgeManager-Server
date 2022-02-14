using Application.Common.Interfaces;
using Domain.Entities;
using Infastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext, IApplicationDbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<FridgeModel> FridgeModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FridgeProduct> FridgeProducts { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeModelConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductConfiguration());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}