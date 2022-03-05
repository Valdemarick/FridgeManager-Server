using System;
using System.ComponentModel.DataAnnotations;
#nullable enable

namespace Application.Models.Fridge
{
    public class FridgeForManipulation
    {
        [Required(ErrorMessage = "ModelId is required")]
        public Guid ModelId { get; set; }

        [MaxLength(50, ErrorMessage = "Maximum length of OwnerName is 50 characters")]
        public string? OwnerName { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum length of Description is 200 characters")]
        public string? Description { get; set; }
    }
}