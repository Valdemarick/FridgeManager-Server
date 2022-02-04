using Domain.Entities;
using Infastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<FridgeModel> FridgeModels { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeModelConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeConfiguration());
            modelBuilder.ApplyConfiguration(new FridgeProductConfiguration());
        }
    }
}