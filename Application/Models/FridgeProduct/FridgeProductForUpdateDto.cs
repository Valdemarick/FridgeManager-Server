using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.FridgeProduct
{
    public class FridgeProductForUpdateDto : FridgeProductForManipulation
    {
        [Required(ErrorMessage = "Id is a required field")]
        public Guid Id { get; set; }
    }
}