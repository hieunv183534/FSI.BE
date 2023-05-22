using AutoMapper.Internal.Mappers;
using BCrypt.Net;
using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Auth.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Account;
using FSI.Domain.Startuper;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
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
using Volo.Abp.DependencyInjection;

namespace FSI.Application.Auth
{
    [RemoteService(false)]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStartuperRepository _startuperRepository;

        public AuthAppService(IAccountRepository accountRepository, IStartuperRepository startuperRepository)
        {
            _accountRepository = accountRepository;
            _startuperRepository = startuperRepository;
        }

        public string Login(LoginDto loginDto)
        {
            var acc = _accountRepository.FindAsync(a => a.Email.Equals(loginDto.Username) || a.PhoneNumber.Equals(loginDto.Username)).Result;
            if (acc == null) return null;

            if (BCrypt.Net.BCrypt.Verify(loginDto.Password, acc.PasswordHash))
            {
                UserRoot user = new UserRoot() {};
                if (true)
                {
                    user = _startuperRepository.FindAsync(f => f.AccountId.Equals(acc.Id)).Result;
                    if (user == null) return null;
                } //... check tiếp các role khác sau


                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("this-is-my-super-key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, "Startuper"),
                        new Claim(ClaimTypes.Email, acc.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.GivenName , acc.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            return null;
        }

        public AccountDto Register(RegisterDto input)
        {
            input.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var newAcc = _accountRepository.InsertAsync(ObjectMapper.Map<RegisterDto, Account>(input)).Result;

            var startuperInfo = ObjectMapper.Map<UserRootDto, FSI.Domain.Startuper.Startuper>(input.BaseInfomation);
            startuperInfo.AccountId = newAcc.Id;
            var userInfo = _startuperRepository.InsertAsync(startuperInfo).Result;

            return ObjectMapper.Map<Account, AccountDto>(newAcc);
        }
    }
}
