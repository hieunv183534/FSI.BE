using FSI.Application.Contracts.Admin.DTO;
using FSI.Application.Contracts.Admin.IService;
using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Common.Enums;
using FSI.Domain.Admin;
using FSI.Domain.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

namespace FSI.Application.Admin
{
    [Authorize]
    public class AdminAppService : ApplicationService, IAdminAppSevice
    {
        private readonly IRepository<Domain.Admin.Admin, Guid> _adminRepository;
        private readonly IProjectRepository _projectRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;
        private string currentAdminPhone;
        public AdminAppService(IRepository<Domain.Admin.Admin, Guid> adminRepository, IHttpContextAccessor httpContextAccessor, IProjectRepository projectRepository)
        {
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
        }

        public async Task AcceptAdmin(Guid adminId)
        {
            var admin =  await _adminRepository.GetAsync(adminId);
            if (admin.Phone.Equals("0971883025"))
                throw new UserFriendlyException(message: "403");

            admin.IsActive = true;
            await _adminRepository.UpdateAsync(admin);
        }

        public async Task DeleteAdmin(Guid adminId)
        {
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var myAdmin = await _adminRepository.GetAsync(currentUserId);
            var admin = await _adminRepository.GetAsync(adminId);
            if (myAdmin.Phone.Equals("0971883025"))
            {
                await _adminRepository.DeleteAsync(admin);
            }
            else
            {
                if (admin.IsActive)
                {
                    throw new UserFriendlyException(message: "403");
                }
                else
                {
                    await _adminRepository.DeleteAsync(admin);
                }
            }
        }

        public async Task<PagedResultDto<AdminDto>> GetListAdmin(bool isActive, string? filter, int skipCount, int maxResultCount)
        {
            this.currentAdminPhone = HttpContext.User.FindFirst("phoneNumber").Value;
            var admins = await _adminRepository.GetQueryableAsync();
            admins = admins.WhereIf(!String.IsNullOrEmpty(filter), x => x.Email.Contains(filter) || x.Phone.Contains(filter) || x.Name.Contains(filter))
                            .Where(x => x.IsActive == isActive && !x.Phone.Equals("0971883025") && !x.Phone.Equals(currentAdminPhone));

            var rs = admins.ToList();
            return new PagedResultDto<AdminDto>()
            {
                TotalCount = rs.Count,
                Items = ObjectMapper.Map<List<Domain.Admin.Admin>, List<AdminDto>>(rs.Skip(skipCount).Take(maxResultCount).ToList())
            };
        }

        [AllowAnonymous]
        public async Task<string> Login(LoginAdminDto input)
        {
            var admin = await _adminRepository.FindAsync(x => x.Phone.Equals(input.Username) || x.Email.Equals(input.Username));
            if (admin == null)
                throw new AbpAuthorizationException();
            if(!admin.IsActive)
                throw new AbpAuthorizationException(code: "FSI");

            if (BCrypt.Net.BCrypt.Verify(input.Password, admin.Password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("this-is-my-super-key");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, admin.Name),
                        new Claim(ClaimTypes.Role, FsiRole.Admin.ToString()),
                        new Claim(ClaimTypes.Email, admin.Email),
                        new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                        new Claim(ClaimTypes.GivenName , admin.Id.ToString()),
                           new Claim("phoneNumber" , admin.Phone),
                        new Claim("avatarUrl" , "../../../../assets/img/profileIcon.png"),
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
        public async Task<AdminDto> Register(RegisterAdminDto input)
        {
            input.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
            var admin = await _adminRepository.InsertAsync(new Domain.Admin.Admin()
            {
                Phone = input.Phone,
                Email= input.Email,
                Name= input.Name,
                Password = input.Password,
                IsActive = false
            });

            return ObjectMapper.Map<FSI.Domain.Admin.Admin, AdminDto>(admin);
        }

        public async Task<PagedResultDto<ProjectDto>> PostToGetListProjectForAdmin(GetListProjectForAdminDto input)
        {
            var projects = await _projectRepository.GetListAsync();

            projects = projects.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.ProjectName.Contains(input.Filter) || x.Description.Contains(input.Filter))
                                .WhereIf(input.Areas.Count != 0, x => input.Areas.Contains(x.Area.Value))
                                .WhereIf(input.Stages.Count != 0, x => input.Stages.Contains(x.Stage.Value))
                                .WhereIf(input.Fields.Count != 0, x => x.Fields.Any(y => input.Fields.Contains(y)))
                                .Where(x=> x.IsActive.Equals(input.IsActive)).ToList();


            var projectPageds = projects.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<ProjectDto>()
            {
                Items = ObjectMapper.Map<List<FSI.Domain.Project.Project>, List<ProjectDto>>(projectPageds),
                TotalCount = projects.Count
            };
        }

        public async Task AcceptProject(Guid projectId)
        {
            var project = await _projectRepository.GetAsync(projectId);
            project.IsActive = true;
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProject(Guid projectId)
        {
            await _projectRepository.DeleteAsync(projectId);
        }
    }
}
