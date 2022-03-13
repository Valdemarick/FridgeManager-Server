using Application.Models.User;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Managers
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);
        Task<string> CreateToken();
    }
}