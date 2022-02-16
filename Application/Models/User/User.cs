using Microsoft.AspNetCore.Identity;

namespace Application.Models.User
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}