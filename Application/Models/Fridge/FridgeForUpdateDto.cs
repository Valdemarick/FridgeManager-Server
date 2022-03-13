using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Fridge
{
    public class FridgeForUpdateDto : FridgeForManipulation
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
    }
}