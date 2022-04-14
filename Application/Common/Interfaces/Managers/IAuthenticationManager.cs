using Application.Models.User;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Managers
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthenticationDto);
        Task<string> CreateTokenAsync();
    }
}