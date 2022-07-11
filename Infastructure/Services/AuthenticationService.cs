﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Services;
using Application.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager,
                                        IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<string> SignInAsync(UserForAuthenticationDto userForAuthenticationDto)
        {
            if (userForAuthenticationDto == null)
            {
                throw new ArgumentNullException(nameof(UserForAuthenticationDto));
            }

            bool isExists = await _authenticationManager.ValidateUserAsync(userForAuthenticationDto);
            if (!isExists)
            {
                throw new NotFoundException("This user does not exist");
            }

            return await _authenticationManager.CreateTokenAsync();
        }

        public async Task SignUpAsync(UserForRegistrationDto userForRegistrationDto)
        {
            if (userForRegistrationDto == null)
            {
                throw new ArgumentNullException(nameof(UserForRegistrationDto));
            }

            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            if (!result.Succeeded)
            {
                throw new ArgumentException("Cannot register this user. Smth went wrong");
            }

            await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
        }
    }
}