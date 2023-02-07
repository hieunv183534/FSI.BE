using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Auth.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace FSI.HttpApi.Controlles
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : AbpController
    {
        private readonly IAuthAppService _authAppService;
        
        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

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
    }
}
