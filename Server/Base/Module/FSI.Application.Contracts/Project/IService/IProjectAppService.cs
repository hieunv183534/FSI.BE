using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.User.DTO;
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

        Task UpdateProjectHistory(Guid projectId,List<ProjectHistoryEventDto> input);

        Task<PagedResultDto<ProjectDto>> PostToGetListProjectForStartuper(GetListProjectForStartuperDto input);

        Task<PagedResultDto<ProjectDto>> PostToGetListProjectForInvestor(GetListProjectForInvestorDto input);

        Task<List<ProjectUserDto>> GetProjectByUser(Guid userId);

        Task<string> UploadAvatar(Guid? projectId);

        Task UploadFile(Guid? projectId, string fileTitle, string note, bool visibleForInvestor, bool visibleForAll);

        Task<List<ProjectFileDto>> GetProjectFiles(Guid projectId);

        Task<List<ProjectUserDto>> GetUsersOfProject(Guid projectId);

        Task<UserRootDto> GetUserByUserNameForInviteToProject(string userName, Guid projectId);

        Task<List<ProjectUserDto>> GetUsersRequestToProject(Guid projectId);

        Task<List<ProjectUserDto>> GetUsersProjectRequestTo(Guid projectId);

        Task RequestToUserFromProject(Guid userId, Guid projectId);

        Task RequestToProject(Guid projectId);

        Task AcceptRequestFromAProject(Guid projectId);

        Task CancelRequestToAProject(Guid projectId);

    }
}
