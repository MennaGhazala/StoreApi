using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared;
using Shared.IdentityDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<User> userManager,
        IMapper mapper, IOptions<JwtOptions> options) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email.ToUpper());
            if(user is null)
            throw new UnAuthorizedException($"emai :{loginDto.Email} does not Exist !");

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnAuthorizedException($"password :{loginDto.Password} not right  !");

            return new UserResultDto
            (

                user.DisplayName,
                user.Email,
                await CreatTokenAsync(user)

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
                await CreatTokenAsync(user)

           );
        }


        public async Task<string> CreatTokenAsync(User user) 
        {//claims
            var jwtOptions =options.Value;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),

            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));
        
            var creds =new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires:DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issuer

                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

      

        

        public async Task<bool> IsEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
           return user !=null;
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(x=>x.Address)
                .FirstOrDefaultAsync(x=>x.Email==email);
            if (user is null)
                throw new UserNoyFound(email);

            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await userManager.Users.Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
                throw new UserNoyFound(email);
          
            var mappedAddress= mapper.Map<Address>(addressDto);
                user.Address = mappedAddress;
            
            
            await userManager.UpdateAsync(user);
            return addressDto;
        }

        public async Task<UserResultDto> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                throw new UserNoyFound(email);

            return new UserResultDto(
                user.DisplayName,
                user.Email,
                null

                );
        }
    }

}
