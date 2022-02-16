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
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, 
                                        UserManager<User> userManager /*RoleManager<IdentityRole> roleManager*/)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            //_roleManager = roleManager;
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

            //foreach(var role in userForRegistrationDto.Roles)
            //{
            //    if (!await _roleManager.RoleExistsAsync(role))
            //    {
            //        _logger.LogInfo($"Role {role} doesn't exist in the database");
            //        return NotFound();
            //    }
            //}

            await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);

            return StatusCode(201);
        }
    }
}