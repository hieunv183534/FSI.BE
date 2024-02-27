using AutoMapper.Internal.Mappers;
using BCrypt.Net;
using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Auth.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Account;
using FSI.Domain.Investor;
using FSI.Domain.Startuper;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace FSI.Application.Auth
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStartuperRepository _startuperRepository;
        private readonly IInvestorRepository _investorRepository;

        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthAppService(IAccountRepository accountRepository, IStartuperRepository startuperRepository, IInvestorRepository investorRepository, IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _startuperRepository = startuperRepository;
            _investorRepository = investorRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> ChangePassword(string oldPass, string newPass)
        {
            var accId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.GivenName).Value);
            var acc = await _accountRepository.GetAsync(accId);

            if (BCrypt.Net.BCrypt.Verify(oldPass, acc.PasswordHash))
            {
                acc.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPass);
                await _accountRepository.UpdateAsync(acc);
                return true;
            }
            else return false;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Login(LoginDto loginDto)
        {
            var acc = await _accountRepository.FindAsync(a => a.Email.Equals(loginDto.Username) || a.PhoneNumber.Equals(loginDto.Username));
            if (acc == null) throw new AbpAuthorizationException();

            if (BCrypt.Net.BCrypt.Verify(loginDto.Password, acc.PasswordHash))
            {
                UserRoot user = new UserRoot() { };
                if (loginDto.Role == Common.Enums.FsiRole.Startuper)
                {
                    user = await _startuperRepository.FindAsync(f => f.AccountId.Equals(acc.Id));
                    if (user == null) return null;
                }
                else if (loginDto.Role == Common.Enums.FsiRole.Investor)
                {
                    user = await _investorRepository.FindAsync(f => f.AccountId.Equals(acc.Id));
                    if (user == null) return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("this-is-my-super-key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, loginDto.Role.ToString()),
                        new Claim(ClaimTypes.Email, acc.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.GivenName , acc.Id.ToString()),
                        new Claim("phoneNumber" , acc.PhoneNumber),
                        new Claim("job" , user.Job.ToString()),
                        new Claim("avatarUrl" , user.AvatarUrl ?? "../../../../assets/img/profileIcon.png"),
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {
                throw new AbpAuthorizationException();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AccountDto> Register(RegisterDto input)
        {
            input.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var newAcc = await _accountRepository.InsertAsync(ObjectMapper.Map<RegisterDto, Account>(input), autoSave: true);

            if (input.RoleRegister == Common.Enums.FsiRole.Startuper)
            {
                var startuperInfo = ObjectMapper.Map<UserRootDto, FSI.Domain.Startuper.Startuper>(input.BaseInfomation);
                startuperInfo.AccountId = newAcc.Id;
                startuperInfo.IsNewProfile = true;
                var userInfo = await _startuperRepository.InsertAsync(startuperInfo);
            }
            else if (input.RoleRegister == Common.Enums.FsiRole.Investor)
            {
                var investorInfo = ObjectMapper.Map<UserRootDto, FSI.Domain.Investor.Investor>(input.BaseInfomation);
                investorInfo.AccountId = newAcc.Id;
                investorInfo.IsNewProfile = true;
                var userInfo = await _investorRepository.InsertAsync(investorInfo);
            }
            return ObjectMapper.Map<Account, AccountDto>(newAcc);
        }
    }
}
