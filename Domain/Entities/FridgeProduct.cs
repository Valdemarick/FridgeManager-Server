using System;
#nullable enable

namespace Domain.Entities
{
    public class FridgeProduct
    {
        public Guid FridgeId { get; set; }
        public Fridge Fridge { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int ProductQuantity { get; set; }
    }
}
