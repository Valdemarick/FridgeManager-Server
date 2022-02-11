#nullable enable

using System;

namespace Application.Models.Fridge
{
    public class FridgeForCreationDto
    {
        public string? OwnerName { get; set; }
        public Guid ModelId { get; set; }
    }
}
