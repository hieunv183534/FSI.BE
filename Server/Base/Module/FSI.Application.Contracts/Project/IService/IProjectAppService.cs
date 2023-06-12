using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Project.IService
{
    public interface IProjectAppService
    {
        Task<ProjectDto> InsertProjectAsync(CreateProjectDto input);

        Task<ProjectDto> UpdateProjectAsync(CreateProjectDto input);

        Task AddUserToProject(AddUserToProjectDto input);

        Task<ProjectDto> GetProjectById(Guid projectId);

        Task AddProjectHistoryEvent(AddProjectHistoryEventDto input);

        Task<PagedResultDto<ProjectDto>> GetListProjectForStartuper(GetListProjectForStartuperDto input);

        Task<PagedResultDto<ProjectDto>> GetListProjectForInvestor(GetListProjectForInvestorDto input);

        Task<List<ProjectUserDto>> GetProjectByUser(Guid userId);

        Task UploadAvatar(Guid? projectId);

    }
}
