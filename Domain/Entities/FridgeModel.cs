using Domain.Common;
using System.Collections.Generic;
#nullable enable

namespace Domain.Entities
{
    public class FridgeModel : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int? ProductionYear { get; set; }
        public List<Fridge> Fridges { get; set; } = new();
    }
}