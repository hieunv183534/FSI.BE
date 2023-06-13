using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
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
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        public ProjectAppService(IProjectRepository projectRepository, IRepository<ProjectUser, Guid> projectUserRepository, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
            _fileInfomationRepository = fileInfomationRepository;
            _accountRepository = accountRepository;
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
                ProjectName = input.ProjectName,
                IsHireNewMember = input.IsHireNewMember,
                AvailableTimeRequire = input.AvailableTimeRequire,
                FounderId = this.currentUserId
            });

            var projectUser = await _projectUserRepository.InsertAsync(new ProjectUser()
            {
                IsActive = true,
                ProjectId = project.Id,
                Role = Common.Enums.RoleInProject.Founder,
                UserId = this.currentUserId
            });

            var rs = ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);
            return rs;
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

        public async Task UpdateProjectHistory(Guid projectId, List<ProjectHistoryEventDto> input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new BusinessException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new BusinessException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(projectId);
            project.History = ObjectMapper.Map<List<ProjectHistoryEventDto>, List<ProjectHistoryEvent>>(input);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<PagedResultDto<ProjectDto>> GetListProjectForStartuper(GetListProjectForStartuperDto input)
        {
            var projects = (await _projectRepository.GetListProjectForStartuper(input.Filter, input.Area, input.Field, input.Stage, input.AvailableTime))
                .WhereIf(input.Field.HasValue, x => x.Fields.Contains(input.Field.Value))
                .WhereIf(input.AvailableTime.HasValue, x => x.AvailableTimeRequire.Contains(input.AvailableTime.Value)).ToList();

            var myProjectIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(this.currentUserId))).Select(x => x.ProjectId).ToList();

            if (input.IsMyProject.Value)
            {
                projects = projects.Where(x => myProjectIds.Contains(x.Id)).ToList();
            }
            else
            {
                projects = projects.Where(x => !myProjectIds.Contains(x.Id)).ToList();
            }

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

        public async Task UploadAvatar(Guid? projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new BusinessException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new BusinessException(message: "Bạn không đủ quyền");

            var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images"),
                fileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = "http://localhost:7777/images/" + fileName;

            await _fileInfomationRepository.InsertAsync(new FileInfomation()
            {
                AuthorId = this.currentUserId,
                Url = fileUrl,
                Size = (int)file.Length
            });

            var project = await _projectRepository.GetAsync(projectId.Value);
            project.AvatarUrl = fileUrl;
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<List<ProjectUserDto>> GetUsersOfProject(Guid projectId)
        {
            var projectUsers = await _projectUserRepository.GetQueryableAsync();
            var users = await _userRepository.GetQueryableAsync();

            var query = from pu in projectUsers
                        join u in users
                        on pu.UserId equals u.Id
                        where pu.ProjectId.Equals(projectId)
                        select pu;

            var usersOfProject = query.ToList();

            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(usersOfProject);

        }

        public async Task<UserRootDto> GetUserByUserNameForInviteToProject(string userName, Guid projectId)
        {
            var account = await _accountRepository.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(userName) || x.Email.Equals(userName));


        }
    }
}
