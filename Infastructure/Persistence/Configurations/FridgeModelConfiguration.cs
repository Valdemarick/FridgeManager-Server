using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Persistence.Configurations
{
    public class FridgeModelConfiguration : IEntityTypeConfiguration<FridgeModel>
    {
        public void Configure(EntityTypeBuilder<FridgeModel> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(pt => pt.Id).ValueGeneratedOnAdd();

            builder.HasCheckConstraint("ProductionYear", "ProductionYear >= 1899 AND ProductionYear <= 2022");

            builder.Property(pt => pt.Name).HasMaxLength(30).IsRequired();

            builder.HasIndex(pt => pt.Name).IsUnique();
        }
    }
}