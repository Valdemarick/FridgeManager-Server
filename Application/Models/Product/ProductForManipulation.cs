using System.ComponentModel.DataAnnotations;
#nullable enable

namespace Application.Models.Product
{
    public class ProductForManipulation
    {
        [Required(ErrorMessage = "'Name' property is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for 'Name' property is 30 characters")]
        public string Name { get; set; } = null!;

        [Range(0, 10, ErrorMessage = "'Quantity' property can't be less than 0 and more than 10'")]
        public int Quantity { get; set; }
    }
}