using Application.Common.Interfaces.Services;
using Application.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userForRegistrationDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _authenticationService.SignUpAsync(userForRegistrationDto);
            return StatusCode(201);
        }

        /// <summary>
        /// Authentication. If the authentication is successful, it will return a new JWT 
        /// </summary>
        /// <param name="userForAuthenticationDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var token = await _authenticationService.SignInAsync(userForAuthenticationDto);
            return Ok(new { Token = token });
        }
    }
}