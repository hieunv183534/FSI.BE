using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Auth.IService;
using FSI.Application.Contracts.CommonDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace FSI.HttpApi.Controlles
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : AbpController
    {
        private readonly IAuthAppService _authAppService;
        
        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto account)
        {
            var token = _authAppService.Login(account);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto account)
        {
            var rs = _authAppService.Register(account);
            return Ok(rs);
        }
    }
}
