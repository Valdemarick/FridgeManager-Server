using System;
#nullable enable

namespace Application.Models.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Quantity { get; set; }
    }
}
