using System;
#nullable enable

namespace Application.Models.FridgeModel
{
    public class FridgeModelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ProductionYear { get; set; }
    }
}
