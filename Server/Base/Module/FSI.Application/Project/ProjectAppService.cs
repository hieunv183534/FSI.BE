using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Project
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IRepository<ProjectFile, Guid> _projectFileRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        public ProjectAppService(IProjectRepository projectRepository, IRepository<ProjectUser, Guid> projectUserRepository, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository, IRepository<ProjectFile, Guid> projectFileRepository)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
            _fileInfomationRepository = fileInfomationRepository;
            _accountRepository = accountRepository;
            _projectFileRepository = projectFileRepository;
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
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.Id));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(input.Id.Value);

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
            project.IsHireNewMember = input.IsHireNewMember;
            project.AvailableTimeRequire = input.AvailableTimeRequire;

            var rs = await _projectRepository.UpdateAsync(project);
            return ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(rs);
        }

        public async Task AddUserToProject(AddUserToProjectDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này1");

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
                throw new UserFriendlyException(message: "Bạn không đủ quyền");
            }
        }

        public async Task<ProjectDto> GetProjectById(Guid projectId)
        {
            var users = await _userRepository.GetListAsync();
            var project = await _projectRepository.GetAsync(projectId);
            var membersAndInvestor = await _projectUserRepository.GetListAsync(x => x.IsActive && x.ProjectId.Equals(projectId));
            var memberCount = membersAndInvestor.Where(x => x.Role != Common.Enums.RoleInProject.Investor).Count();
            var totalInvesment = membersAndInvestor.Where(x => x.Role == Common.Enums.RoleInProject.Investor).Select(x => x.TotalInvestment).Sum();
            var rs = ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);
            rs.SetProperty("memberCount", memberCount);
            rs.SetProperty("totalInvesment", totalInvesment);
            return rs;
        }

        public async Task UpdateProjectHistory(Guid projectId, List<ProjectHistoryEventDto> input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(projectId);
            project.History = ObjectMapper.Map<List<ProjectHistoryEventDto>, List<ProjectHistoryEvent>>(input);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<PagedResultDto<ProjectDto>> PostToGetListProjectForStartuper(GetListProjectForStartuperDto input)
        {
            var projects = await _projectRepository.GetListAsync();

            projects = projects.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.ProjectName.Contains(input.Filter) || x.Description.Contains(input.Filter))
                                .WhereIf(input.Areas.Count != 0 , x => input.Areas.Contains(x.Area.Value))
                                .WhereIf(input.Stages.Count != 0, x => input.Stages.Contains(x.Stage.Value))
                                .WhereIf(input.Fields.Count != 0, x => x.Fields.Any(y => input.Fields.Contains(y)))
                                .WhereIf(input.AvailableTimes.Count != 0, x => x.AvailableTimeRequire.Any(y => input.AvailableTimes.Contains(y))).ToList();

            var myProjectIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(this.currentUserId))).Select(x => x.ProjectId).ToList();

            if (input.IsMyProject.Value)
            {
                projects = projects.Where(x => myProjectIds.Contains(x.Id)).ToList();
            }
            else
            {
                projects = projects.Where(x => !myProjectIds.Contains(x.Id)).ToList();
            }

            var projectPageds = projects.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var projectUsers = await _projectUserRepository.GetListAsync();

            projectPageds.ForEach(async p =>
            {
                var membersAndInvestor = projectUsers.Where(x => x.IsActive && x.ProjectId.Equals(p.Id)).ToList();
                p.SetProperty("memberCount", membersAndInvestor.Where(x => x.Role != Common.Enums.RoleInProject.Investor).Count());
                p.SetProperty("totalInvesment", membersAndInvestor.Where(x => x.Role == Common.Enums.RoleInProject.Investor).Select(x => x.TotalInvestment).Sum());
            });

            return new PagedResultDto<ProjectDto>()
            {
                Items = ObjectMapper.Map<List<FSI.Domain.Project.Project>, List<ProjectDto>>(projectPageds),
                TotalCount = projects.Count
            };
        }

        public async Task<PagedResultDto<ProjectDto>> PostToGetListProjectForInvestor(GetListProjectForInvestorDto input)
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

        public async Task<List<ProjectUserDto>> GetUsersOfProject(Guid projectId)
        {
            var users = await _userRepository.GetListAsync();

            var usersOfProject = await _projectUserRepository.GetListAsync(x => x.ProjectId.Equals(projectId) &&
                                                                                x.IsActive && x.Role != Common.Enums.RoleInProject.Investor);

            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(usersOfProject);

        }

        public async Task<UserRootDto> GetUserByUserNameForInviteToProject(string userName, Guid projectId)
        {
            var account = await _accountRepository.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(userName) || x.Email.Equals(userName));

            if (account == null)
                throw new UserFriendlyException(message: "Không tìm thấy người dùng!");

            var user = await _userRepository.FirstOrDefaultAsync(x => x.AccountId.Equals(account.Id));

            var userProject = await _projectUserRepository.FirstOrDefaultAsync(x => x.UserId.Equals(user.Id) && x.ProjectId.Equals(projectId));

            if (userProject != null)
                throw new UserFriendlyException(message: "Người dùng đã thuộc dự án!");

            return ObjectMapper.Map<UserRoot, UserRootDto>(user);
        }

        public async Task<string> UploadAvatar(Guid? projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");

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
                Size = (int)file.Length,
                ContentType = file.ContentType
            });

            var project = await _projectRepository.GetAsync(projectId.Value);
            project.AvatarUrl = fileUrl;
             var rs =await _projectRepository.UpdateAsync(project);
            return rs.AvatarUrl;
        }

        public async Task UploadFile(Guid? projectId, string fileTitle, string note)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];

            string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"Docs"),
                file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileInfo = await _fileInfomationRepository.InsertAsync(new FileInfomation()
            {
                AuthorId = this.currentUserId,
                Url = file.FileName,
                Size = (int)file.Length,
                ContentType = file.ContentType
            });

            await _projectFileRepository.InsertAsync(new ProjectFile()
            {
                ProjectId = projectId.Value,
                FileId = fileInfo.Id,
                Note = note,
                Title = fileTitle
            });
        }

        public async Task<List<ProjectUserDto>> GetUsersRequestToProject(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");
            var users = await _userRepository.GetListAsync();
            var projectUsers = await _projectUserRepository.GetListAsync(x => !x.IsActive &&
                                                                                x.ProjectId.Equals(projectId) &&
                                                                                x.IsFromUser &&
                                                                                x.Role != Common.Enums.RoleInProject.Investor);
            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(projectUsers);
        }

        public async Task<List<ProjectUserDto>> GetUsersProjectRequestTo(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");
            var users = await _userRepository.GetListAsync();
            var projectUsers = await _projectUserRepository.GetListAsync(x => !x.IsActive &&
                                                                                x.ProjectId.Equals(projectId) &&
                                                                                !x.IsFromUser &&
                                                                                x.Role != Common.Enums.RoleInProject.Investor);
            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(projectUsers);
        }

        public async Task RequestToUserFromProject(Guid userId, Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");

            var myProjectUser1 = await _projectUserRepository.FindAsync(x => x.UserId.Equals(userId) && x.ProjectId.Equals(projectId));
            if (myProjectUser1 != null)
                throw new UserFriendlyException(message: "Người dùng đã thuộc về dự án hoặc đã gửi request!");

            await _projectUserRepository.InsertAsync(new ProjectUser()
            {
                ProjectId = projectId,
                UserId = userId,
                IsActive = false,
                IsFromUser = false,
                Role = Common.Enums.RoleInProject.CoFounder
            });
        }

        public async Task RequestToProject(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser != null)
                throw new UserFriendlyException(message: "Bạn đã thuộc về dự án hoặc đã gửi request!");

            await _projectUserRepository.InsertAsync(new ProjectUser()
            {
                ProjectId = projectId,
                UserId = this.currentUserId,
                IsActive = false,
                IsFromUser = true,
                Role = Common.Enums.RoleInProject.CoFounder
            });
        }

        public async Task<List<ProjectFileDto>> GetProjectFiles(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            var files = await _fileInfomationRepository.GetListAsync();
            var projectFiles = await _projectFileRepository.GetListAsync(x => x.ProjectId.Equals(projectId), includeDetails: true);
            return ObjectMapper.Map<List<ProjectFile>, List<ProjectFileDto>>(projectFiles);
        }
    }
}
