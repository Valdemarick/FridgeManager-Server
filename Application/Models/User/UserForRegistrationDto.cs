using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable enable

namespace Application.Models.User
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+" + 
            @"(\.[a-z0-9_-]+)*.[a-z]{2,6}$", ErrorMessage = "You entered a wrong email address")]
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ICollection<string> Roles { get; set; } = null!;
    }
}