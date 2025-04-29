using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.IdentityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager serviceManager) :ApiController
    {
        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(loginDto));

        [HttpPost]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
                => Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDto));


    }
}
