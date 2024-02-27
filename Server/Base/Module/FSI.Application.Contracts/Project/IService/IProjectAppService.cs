using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.DTO.Hiring;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using FSI.Domain.Project;
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
        Task TestAzureRedis(string value);

        Task<ProjectDto> InsertProjectAsync(CreateProjectDto input);

        Task<ProjectDto> PostUpdateProjectAsync(CreateProjectDto input);

        Task AddUserToProject(AddUserToProjectDto input);

        Task<ProjectDto> GetProjectById(Guid projectId);

        Task<List<ProjectDto>> GetTopProjectSimilarByProjectId(Guid projectId);

        Task<PagedResultDto<ProjectDto>> PostToGetListProjectForStartuper(GetListProjectForStartuperDto input);

        Task<PagedResultDto<ProjectDto>> PostToGetListProjectForInvestor(GetListProjectForInvestorDto input);

        Task<List<ProjectUserDto>> GetProjectByUser(Guid userId);

        Task<string> UploadAvatar(Guid? projectId);

        Task<Guid> UploadFile(Guid? projectId, string fileTitle, string note, bool visibleForInvestor, bool visibleForAll);

        Task<List<ProjectFileDto>> GetProjectFiles(Guid projectId);

        Task<List<ProjectUserDto>> GetUsersOfProject(Guid projectId);

        Task<UserRootDto> GetUserByUserNameForInviteToProject(string userName, Guid projectId);

        Task<List<ProjectUserDto>> GetUsersRequestToProject(Guid projectId);

        Task<List<ProjectUserDto>> GetUsersProjectRequestTo(Guid projectId);

        Task RequestToUserFromProject(Guid userId, Guid projectId);

        Task RequestToProject(Guid projectId);

        Task AcceptRequestFromAProject(Guid projectId);

        Task CancelRequestToAProject(Guid projectId);

        Task AcceptMemberToProject(Guid projectId, Guid userId);

        Task RefuseMemberToProject(Guid projectId, Guid userId);

        Task AddPostToProject(PostToProjectDto input);

        Task<PagedResultDto<ProjectEventDto>> PostToGetEventsOfProject(GetProjectEventsDto input);

        Task<List<ProjectCalendarEventDto>> GetProjectCalendarEvents(Guid projectId);

        Task AddCalendarEvent(AddProjectCalendarEventDto input);

        Task DeleteCalendarEvent(Guid calendarEventId);

        Task<ProjectWorkDto> AddWork(AddProjectWorkDto input);

        Task ChangeWorkStatus(Guid workId, WorkStatus newStatus);

        Task<List<ProjectWorkDto>> GetProjectWorks(Guid projectId);

        Task<ProjectRequestStartuperInfoDto> GetProjectRequestStartuperInfo(Guid projectId);

        Task PostUpdateProjectRequestStartuperInfo(ProjectRequestStartuperInfoDto input);

        Task<List<ProjectHiringDto>> GetProjectHirings(Guid projectId);

        Task<ProjectHiringDto> GetProjectHiring(Guid projectId, Guid hiringId);

        Task CreateProjectHiring(CreateOrUpdateProjectHiringDto input);

        Task UpdateProjectHiring(CreateOrUpdateProjectHiringDto input);

        Task DeleteProjectHiring(Guid projectId, Guid hiringId);

        Task<string> GetProjectCanvasModel(Guid projectId);

        Task UpdateProjectCanvasModel(UpdateCanvasModelDto input);

        Task<List<Pitch>> UploadPitchDeck(Guid projectId);

        Task<List<Pitch>> GetProjectPitchDeck(Guid projectId);

        Task<List<Pitch>> DeletePitchDeck(Guid projectId, Guid pitchId);

        Task<List<Pitch>> SortPitchDeck(Guid projectId ,List<Pitch> pitchSorteds);

    }
}
