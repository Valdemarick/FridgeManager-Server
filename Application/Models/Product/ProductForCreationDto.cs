#nullable enable

namespace Application.Models.Product
{
    public class ProductForCreationDto
    {
        public string Name { get; set; } = null!;
        public int? Quantity { get; set; }
    }
}
