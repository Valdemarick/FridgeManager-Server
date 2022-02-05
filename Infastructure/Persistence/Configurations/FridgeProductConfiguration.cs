using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infastructure.Persistence.Configurations
{
    public class FridgeProductConfiguration : IEntityTypeConfiguration<FridgeProduct>
    {
        public void Configure(EntityTypeBuilder<FridgeProduct> builder)
        {
            builder.Property(pt => pt.ProductQuantity).IsRequired();
            builder.Ignore(pt => pt.Id);
        }
    }
}