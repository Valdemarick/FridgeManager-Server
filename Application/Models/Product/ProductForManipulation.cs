using System.ComponentModel.DataAnnotations;

namespace Application.Models.Product
{
    public class ProductForManipulation
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Maximum length of Name is 30 characters")]
        public string Name { get; set; } 

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 9, ErrorMessage = "Quantity can't be less than 1 and more than 9")]
        public int Quantity { get; set; }
    }
}