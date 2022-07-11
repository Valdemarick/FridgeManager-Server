using Domain.Common;
using System;
using System.Collections.Generic;
#nullable enable

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public List<Fridge> Fridges { get; set; } = new();
        public List<FridgeProduct> FridgeProducts { get; set; } = new();
    }
}