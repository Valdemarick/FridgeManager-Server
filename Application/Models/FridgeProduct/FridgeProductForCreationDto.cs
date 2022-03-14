using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.FridgeProduct
{
    public class FridgeProductForCreationDto
    {
        [Required(ErrorMessage ="'FridgeId' prperty is a required field")]
        public Guid FridgeId { get; set; }

        [Required(ErrorMessage ="'ProductId' property is a required field")]
        public Guid ProductId { get; set; }

        //[Required(ErrorMessage ="'ProductQuantity' property is a required field")]
        [Range(0, 9, ErrorMessage ="'ProductQuantity' property can't be less than 0 and more than 9")]
        public int? ProductQuantity { get; set; }
    }
}