using System;
using System.ComponentModel.DataAnnotations;
#nullable enable

namespace Application.Models.Fridge
{
    public class FridgeForManipulation
    {
        [Required(ErrorMessage = "'ModelId' property is a required field")]
        public Guid ModelId { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum length for 'OwnerName' property is 50 characters")]
        public string? OwnerName { get; set; }
    }
}