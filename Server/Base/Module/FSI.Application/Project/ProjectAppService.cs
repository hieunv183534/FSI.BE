using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.IService;
using FSI.Domain.Project;
using FSI.Domain.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Project
{
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IUserRootRepository _userRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        public ProjectAppService(IProjectRepository projectRepository, IRepository<ProjectUser, Guid> projectUserRepository, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
        }

        public async Task<ProjectDto> InsertProjectAsync(CreateProjectDto input)
        {
            var project = await _projectRepository.InsertAsync(new Domain.Project.Project()
            {
                Area = input.Area,
                AvatarUrl = input.AvatarUrl,
                Compliment = input.Compliment,
                Description = input.Description,
                Fb = input.Fb,
                Fields = input.Fields,
                History = input.History,
                Stage = input.Stage,
                Website = input.Website,
                FoundedTime = input.FoundedTime,
                ProjectName = input.ProjectName
            });

            var projectUser = await _projectUserRepository.InsertAsync(new ProjectUser()
            {
                IsActive = true,
                ProjectId = project.Id,
                Role = Common.Enums.RoleInProject.Founder,
                UserId = this.currentUserId
            });

            return ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateProjectAsync(CreateProjectDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null)
                throw new BusinessException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này1");

            if ((int)myProjectUser.Role < 2)
                throw new BusinessException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(input.ProjectId.Value);

            project.ProjectName = input.ProjectName;
            project.AvatarUrl = input.AvatarUrl;
            project.Compliment = input.Compliment;
            project.Area = input.Area;
            project.FoundedTime = input.FoundedTime;
            project.Fb = input.Fb;
            project.Description = input.Description;
            project.AvatarUrl = input.AvatarUrl;
            project.History = input.History;
            project.Stage = input.Stage;
            project.Website = input.Website;

            var rs = await _projectRepository.UpdateAsync(project);
            return ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(rs);
        }

        public async Task AddUserToProject(AddUserToProjectDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null)
                throw new BusinessException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này1");

            if ((int)myProjectUser.Role >= 2 && (myProjectUser.Role > input.Role))
            {
                var newProjectUser = await _projectUserRepository.InsertAsync(new ProjectUser()
                {
                    Role = input.Role,
                    ProjectId = input.ProjectId,
                    IsActive = true,
                    UserId = input.UserId
                });
            }
            else
            {
                throw new BusinessException(message: "Bạn không đủ quyền");
            }
        }

        public async Task<ProjectDto> GetProjectById(Guid projectId)
        {
            var users = await _userRepository.GetListAsync();
            var project = await _projectRepository.GetAsync(projectId);
            return ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);
        }

        public async Task AddProjectHistoryEvent(AddProjectHistoryEventDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null)
                throw new BusinessException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này1");

            if ((int)myProjectUser.Role < 2)
                throw new BusinessException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(input.ProjectId);
            project.History.Add(ObjectMapper.Map<AddProjectHistoryEventDto, ProjectHistoryEvent>(input));
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<PagedResultDto<ProjectDto>> GetListProjectForStartuper(GetListProjectForStartuperDto input)
        {
            var projects = await _projectRepository.GetListAsync(x => x.ProjectName.Contains(input.Filter) &&
                                                                     x.Stage == input.Stage &&
                                                                     x.Area.Equals(input.Area));

            return new PagedResultDto<ProjectDto>()
            {
                Items = ObjectMapper.Map<List<FSI.Domain.Project.Project>, List<ProjectDto>>(projects.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
                TotalCount = projects.Count
            };
        }

        public async Task<PagedResultDto<ProjectDto>> GetListProjectForInvestor(GetListProjectForInvestorDto input)
        {
            var projects = await _projectRepository.GetListAsync(x => x.ProjectName.Contains(input.Filter) &&
                                                                     x.Stage == input.Stage &&
                                                                     x.Area.Equals(input.Area));

            return new PagedResultDto<ProjectDto>()
            {
                Items = ObjectMapper.Map<List<FSI.Domain.Project.Project>, List<ProjectDto>>(projects.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
                TotalCount = projects.Count
            };
        }

        public async Task<List<ProjectUserDto>> GetProjectByUser(Guid userId)
        {
            var projects = await _projectRepository.GetListAsync();
            var projectUsers = await _projectUserRepository.GetListAsync(x => x.UserId.Equals(userId));

            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(projectUsers);
        }
    }
}
