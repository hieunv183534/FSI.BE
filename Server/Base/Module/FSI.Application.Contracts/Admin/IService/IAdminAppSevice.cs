using FSI.Application.Contracts.Admin.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Domain.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Admin.IService
{
    public interface IAdminAppSevice
    {
        Task<string> Login(LoginAdminDto input);

        Task<AdminDto> Register(RegisterAdminDto input);

        Task<PagedResultDto<AdminDto>> GetListAdmin(bool isActive, string? filter, int skipCount, int maxResultCount);

        Task AcceptAdmin(Guid adminId);

        Task DeleteAdmin(Guid adminId);


        Task<PagedResultDto<ProjectDto>> PostToGetListProjectForAdmin(GetListProjectForAdminDto input);

        Task AcceptProject(Guid projectId);

        Task DeleteProject(Guid projectId);
    }
}
