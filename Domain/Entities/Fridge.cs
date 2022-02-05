using System;
using System.Collections.Generic;
#nullable enable

namespace Domain.Entities
{
    public class Fridge : BaseEntity
    {
        public string? OwnerName { get; set; }

        public Guid FridgeModelId { get; set; }
        public FridgeModel FridgeModel { get; set; } = null!;

        public List<Product> Products { get; set; } = new();
        public List<FridgeProduct> FridgeProducts { get; set; } = new();
    }
}
