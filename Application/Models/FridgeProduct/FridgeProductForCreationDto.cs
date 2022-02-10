using System;

namespace Application.Models.FridgeProduct
{
    public class FridgeProductForCreationDto
    {
        public Guid FridgeId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}