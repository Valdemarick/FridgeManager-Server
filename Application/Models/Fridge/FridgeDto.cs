using System;
#nullable enable

namespace Application.Models.Fridge
{
    public class FridgeDto
    {
        public Guid Id { get; set; }
        public string? OwnerName { get; set; }
        public string Manufacturer { get; set; } = null!;
        public int? ProductionYear { get; set; }
    }
}
