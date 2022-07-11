using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Product
{
    public class ProductForUpdateDto : ProductForManipulation
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
    }
}