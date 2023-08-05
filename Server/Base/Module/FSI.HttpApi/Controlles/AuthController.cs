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
    [IgnoreAntiforgeryToken]
    public class AuthController : AbpController
    {
        private readonly IAuthAppService _authAppService;
        
        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto account)
        {
            var token = await _authAppService.Login(account);
            if (token == null)
            {
                return Unauthorized();
            }
            else if(token == "null")
            {
                return BadRequest("Bạn chưa đăng ký thông tin!");
            }
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("testcicd")]
        public async Task<IActionResult> ABC()
        {
            return Ok("hahahaa");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto account)
        {
            var rs = await _authAppService.Register(account);
            return Ok(rs);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto input)
        {
            var rs = await _authAppService.ChangePassword(input.OldPassword, input.NewPassword);
            return Ok(rs);
        }
 
    }
}
