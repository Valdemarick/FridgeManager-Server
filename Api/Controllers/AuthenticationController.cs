using Application.Common.Interfaces;
using Application.Models.User;
using AutoMapper;
using Infastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, UserManager<User> userManager,
            IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            if (userForRegistrationDto == null)
            {
                _logger.LogError("The sent object is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid model state for 'UserForRegistrationDto' object");
                return UnprocessableEntity(ModelState);
            }

            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.TryAddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
        {
            if (userForAuthenticationDto == null)
            {
                _logger.LogError("The sent object is null");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarn("Invalid model state for 'UserForAuthenticationDto' object");
                return UnprocessableEntity(ModelState);
            }

            if (!await _authenticationManager.ValidateUser(userForAuthenticationDto))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Something wrong");
                return Unauthorized();
            }

            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }
    }
}