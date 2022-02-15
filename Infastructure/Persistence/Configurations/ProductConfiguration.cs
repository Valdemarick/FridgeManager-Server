using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasMany(p => p.Fridges)
                .WithMany(f => f.Products)
                .UsingEntity<FridgeProduct>(
            j => j
                .HasOne(pt => pt.Fridge)
                .WithMany(t => t.FridgeProducts)
                .HasForeignKey(pt => pt.FridgeId),
            j => j
                .HasOne(pt => pt.Product)
                .WithMany(t => t.FridgeProducts)
                .HasForeignKey(pt => pt.ProductId),
            j =>
            {
                j.HasKey(t => new { t.FridgeId, t.ProductId });
                j.ToTable("Fridges_Products");
            });

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasCheckConstraint("Quantity", "Quantity > 0 AND Quantity < 10");
            builder.Property(pt => pt.Quantity).HasDefaultValue(1);

            builder.Property(pt => pt.Name).HasMaxLength(30);

            builder.HasIndex(pt => pt.Name).IsUnique();
        }
    }
}