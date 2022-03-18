using Application.Models.User;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(UserForAuthenticationDto userForAuthenticationDto);
        Task SignUpAsync(UserForRegistrationDto userForRegistrationDto);
    }
}