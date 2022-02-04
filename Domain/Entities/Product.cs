using System;
using System.Collections.Generic;
#nullable enable

namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Quantity { get; set; }
        public List<Fridge> Fridges { get; set; } = new();
        public List<FridgeProduct> FridgeProducts { get; set; } = new();
    }
}
