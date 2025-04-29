using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Shared.IdentityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<User> userManager, IMapper mapper) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email.ToUpper());
            if(user is null)
            throw new UnauthorizedAccessException($"emai :{loginDto.Email} does not Exist !");

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnauthorizedAccessException($"password :{loginDto.Password} not right  !");

            return new UserResultDto
            (

                user.DisplayName,
                user.Email,
                 "token"

            );




        }


        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber,

            };
            var result = await  userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(x => x.Description).ToList();
                throw new ValidationException(error);

            }
            return new UserResultDto
           (

               user.DisplayName,
               user.Email,
                "token"

           );
        }
    }

}
