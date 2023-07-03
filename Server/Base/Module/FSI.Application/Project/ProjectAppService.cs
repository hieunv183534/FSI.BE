using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        private readonly IRepository<ProjectEvent, Guid> _projectEventRepository;
        private readonly IRepository<ProjectCalendarEvent, Guid> _projectCalendarEventRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        public ProjectAppService(IProjectRepository projectRepository, IRepository<ProjectUser, Guid> projectUserRepository, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository, IRepository<ProjectFile, Guid> projectFileRepository, IRepository<ProjectEvent, Guid> projectEventRepository, IRepository<ProjectCalendarEvent, Guid> projectCalendarEventRepository)
        {
            _projectRepository = projectRepository;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _userRepository = userRepository;
            _fileInfomationRepository = fileInfomationRepository;
            _accountRepository = accountRepository;
            _projectFileRepository = projectFileRepository;
            _projectEventRepository = projectEventRepository;
            _projectCalendarEventRepository = projectCalendarEventRepository;
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
                Stage = input.Stage,
                Website = input.Website,
                FoundedTime = input.FoundedTime,
                ProjectName = input.ProjectName,
                IsHireNewMember = input.IsHireNewMember,
                AvailableTimeRequire = input.AvailableTimeRequire,
                FounderId = this.currentUserId
            });

            await _projectEventRepository.InsertAsync(new ProjectEvent()
            {
                Type = ProjectEventType.Init,
                EventTime = input.FoundedTime,
                ProjectId = project.Id
            });

            var projectUser = await _projectUserRepository.InsertAsync(new ProjectUser()
            {
                IsActive = true,
                ProjectId = project.Id,
                Role = Common.Enums.RoleInProject.Founder,
                UserId = this.currentUserId,
                JoinTime = DateTime.Now
            });

            var rs = ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);
            return rs;
        }

        public async Task<ProjectDto> UpdateProjectAsync(CreateProjectDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.Id));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            if (!myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            if ((int)myProjectUser.Role < 2)
                throw new UserFriendlyException(message: "Bạn không đủ quyền");

            var project = await _projectRepository.GetAsync(input.Id.Value);

            if (project.Stage != input.Stage)
            {
                await _projectEventRepository.InsertAsync(new ProjectEvent()
                {
                    Type = ProjectEventType.PhaseSwich,
                    EventTime = DateTime.Now,
                    Stage = input.Stage,
                    ProjectId = project.Id
                });
            }

            project.ProjectName = input.ProjectName;
            project.AvatarUrl = input.AvatarUrl;
            project.Compliment = input.Compliment;
            project.Area = input.Area;
            project.FoundedTime = input.FoundedTime;
            project.Fb = input.Fb;
            project.Description = input.Description;
            project.AvatarUrl = input.AvatarUrl;
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
            if (!myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

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
            RelationWithProject relationWithProject;
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
            {
                relationWithProject = RelationWithProject.NotMemberOfProject;
            }
            else
            {
                if (myProjectUser.IsActive)
                {
                    relationWithProject = RelationWithProject.IsMemberOfProject;
                }
                else
                {
                    if (myProjectUser.IsFromUser)
                    {
                        relationWithProject = RelationWithProject.RequestToProject;
                    }
                    else
                    {
                        relationWithProject = RelationWithProject.ProjectRequestTo;
                    }
                }
            }

            rs.SetProperty("relationWithProject", relationWithProject);
            return rs;
        }

        public async Task<PagedResultDto<ProjectDto>> PostToGetListProjectForStartuper(GetListProjectForStartuperDto input)
        {
            var projects = await _projectRepository.GetListAsync();

            projects = projects.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.ProjectName.Contains(input.Filter) || x.Description.Contains(input.Filter))
                                .WhereIf(input.Areas.Count != 0, x => input.Areas.Contains(x.Area.Value))
                                .WhereIf(input.Stages.Count != 0, x => input.Stages.Contains(x.Stage.Value))
                                .WhereIf(input.Fields.Count != 0, x => x.Fields.Any(y => input.Fields.Contains(y)))
                                .WhereIf(input.AvailableTimes.Count != 0, x => x.AvailableTimeRequire.Any(y => input.AvailableTimes.Contains(y))).ToList();

            var myProjectIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(this.currentUserId) && x.IsActive)).Select(x => x.ProjectId).ToList();
            var projectRequestToMeIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(currentUserId) && !x.IsActive && !x.IsFromUser)).Select(x => x.ProjectId).ToList();
            var projectMeRequestToIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(currentUserId) && !x.IsActive && x.IsFromUser)).Select(x => x.ProjectId).ToList();
            var projectUserIds = (await _projectUserRepository.GetListAsync(x => x.UserId.Equals(currentUserId))).Select(x => x.ProjectId).ToList();

            switch (input.RelationWithProject)
            {
                case Common.Enums.RelationWithProject.IsMemberOfProject:
                    projects = projects.Where(x => myProjectIds.Contains(x.Id)).ToList();
                    break;
                case Common.Enums.RelationWithProject.NotMemberOfProject:
                    projects = projects.Where(x => !projectUserIds.Contains(x.Id)).ToList();
                    break;
                case Common.Enums.RelationWithProject.ProjectRequestTo:
                    projects = projects.Where(x => projectRequestToMeIds.Contains(x.Id)).ToList();
                    break;
                case Common.Enums.RelationWithProject.RequestToProject:
                    projects = projects.Where(x => projectMeRequestToIds.Contains(x.Id)).ToList();
                    break;
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
            if (!myProjectUser.IsActive)
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
            var rs = await _projectRepository.UpdateAsync(project);
            return rs.AvatarUrl;
        }

        public async Task<Guid> UploadFile(Guid? projectId, string fileTitle, string note, bool visibleForInvestor, bool visibleForAll)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            if (!myProjectUser.IsActive)
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

            if (visibleForAll) visibleForInvestor = true;

            var pjFile = await _projectFileRepository.InsertAsync(new ProjectFile()
            {
                ProjectId = projectId.Value,
                FileId = fileInfo.Id,
                Note = note,
                Title = fileTitle,
                VisibleForAll = visibleForAll,
                VisibleForInvestor = visibleForInvestor
            });
            return pjFile.Id;
        }

        public async Task<List<ProjectUserDto>> GetUsersRequestToProject(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            if (!myProjectUser.IsActive)
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
            if (!myProjectUser.IsActive)
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
            if (!myProjectUser.IsActive)
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

        public async Task<List<ProjectFileDto>> GetProjectFiles(Guid projectId)
        {
            var files = await _fileInfomationRepository.GetListAsync();
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            var isOfProject = true;
            if (myProjectUser == null) isOfProject = false;

            else if (!myProjectUser.IsActive) isOfProject = false;


            List<ProjectFile> projectFiles = new List<ProjectFile>();
            if (!isOfProject)
            {
                projectFiles = await _projectFileRepository.GetListAsync(x => x.ProjectId.Equals(projectId) && x.VisibleForAll, includeDetails: true);
            }
            else // thuộc về dự án
            {
                if (myProjectUser.Role == Common.Enums.RoleInProject.Investor) // là nhà đầu tư
                {
                    projectFiles = await _projectFileRepository.GetListAsync(x => x.ProjectId.Equals(projectId) && x.VisibleForInvestor, includeDetails: true);
                }
                else // là thành viên phát triển
                {
                    projectFiles = await _projectFileRepository.GetListAsync(x => x.ProjectId.Equals(projectId), includeDetails: true);
                }
            }
            return ObjectMapper.Map<List<ProjectFile>, List<ProjectFileDto>>(projectFiles);
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

        public async Task AcceptRequestFromAProject(Guid projectId)
        {
            var projectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));

            if (projectUser == null)
                throw new UserFriendlyException(message: "Không tìm thấy request!");

            if (projectUser.IsActive)
            {
                throw new UserFriendlyException(message: "Bạn đã là thành viên dự án!");
            }
            else
            {
                if (projectUser.IsFromUser)
                {
                    throw new UserFriendlyException(message: "Request không phải tới bạn, không thể accept!");
                }
                else
                {
                    projectUser.JoinTime = DateTime.Now;
                    projectUser.IsActive = true;
                    await _projectUserRepository.UpdateAsync(projectUser);
                    await _projectEventRepository.InsertAsync(new ProjectEvent()
                    {
                        Type = ProjectEventType.NewMember,
                        EventTime = DateTime.Now,
                        UserId = currentUserId,
                        ProjectId = projectId
                    });
                }
            }
        }

        public async Task CancelRequestToAProject(Guid projectId)
        {
            var projectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));

            if (projectUser == null)
                throw new UserFriendlyException(message: "Không tìm thấy request!");

            if (projectUser.IsActive)
            {
                throw new UserFriendlyException(message: "Bạn đã là thành viên dự án!");
            }
            else
            {
                if (!projectUser.IsFromUser)
                {
                    throw new UserFriendlyException(message: "Request không phải từ bạn, không thể cancel!");
                }
                else
                {
                    await _projectUserRepository.DeleteAsync(projectUser.Id);
                }
            }
        }

        public async Task AcceptMemberToProject(Guid projectId, Guid userId)
        {
            var projectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(userId) && x.ProjectId.Equals(projectId));

            if (projectUser == null)
                throw new UserFriendlyException(message: "Không tìm thấy request!");
            if (projectUser.IsActive)
                throw new UserFriendlyException(message: "Người dùng đã là thành viên dự án!");
            if (!projectUser.IsFromUser)
                throw new UserFriendlyException(message: "Request này không đến từ người dùng!");

            projectUser.JoinTime = DateTime.Now;
            projectUser.IsActive = true;
            await _projectUserRepository.UpdateAsync(projectUser);
            await _projectEventRepository.InsertAsync(new ProjectEvent()
            {
                Type = ProjectEventType.NewMember,
                EventTime = DateTime.Now,
                UserId = userId,
                ProjectId = projectId
            });
        }

        public async Task RefuseMemberToProject(Guid projectId, Guid userId)
        {
            var projectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(userId) && x.ProjectId.Equals(projectId));

            if (projectUser == null)
                throw new UserFriendlyException(message: "Không tìm thấy request!");
            if (projectUser.IsActive)
                throw new UserFriendlyException(message: "Người dùng đã là thành viên dự án!");
            if (!projectUser.IsFromUser)
                throw new UserFriendlyException(message: "Request này không đến từ người dùng!");

            await _projectUserRepository.DeleteAsync(projectUser);
        }

        public async Task AddPostToProject([FromForm] PostToProjectDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var files = _httpContextAccessor.HttpContext.Request.Form.Files.ToList();
            var fileInfos = new List<FileInfomation>();
            files.ForEach(async file =>
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images"),
                fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var fileUrl = "http://localhost:7777/images/" + fileName;
                fileInfos.Add(new FileInfomation()
                {
                    AuthorId = this.currentUserId,
                    Url = fileUrl,
                    Size = (int)file.Length,
                    ContentType = file.ContentType
                });
            });

            await _fileInfomationRepository.InsertManyAsync(fileInfos);

            var images = fileInfos.Select(x => x.Url).ToList();

            await _projectEventRepository.InsertAsync(new ProjectEvent()
            {
                PosterId = currentUserId,
                Content = input.Content,
                Location = input.Location,
                EventTime = DateTime.Now,
                FileIds = JArray.Parse(input.FileIds).ToObject<List<Guid>>(),
                Links = JArray.Parse(input.Links).ToObject<List<string>>(),
                Images = images,
                Type = ProjectEventType.PostNotification,
                ProjectId = input.ProjectId
            });
        }

        public async Task<PagedResultDto<ProjectEventDto>> PostToGetEventsOfProject(GetProjectEventsDto input)
        {
            var users = await _userRepository.GetListAsync();
            var events = await _projectEventRepository.GetListAsync(x => x.ProjectId.Equals(input.ProjectId));
            events = events.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.Content.Contains(input.Filter)).ToList();

            switch (input.Type)
            {
                case -1:
                    break;
                case 0:
                    events = events.Where(x => x.Type == ProjectEventType.Init || x.Type == ProjectEventType.PhaseSwich).ToList();
                    break;
                case 1:
                    events = events.Where(x => x.Type == ProjectEventType.NewMember || x.Type == ProjectEventType.OutMember).ToList();
                    break;
                case 3:
                    events = events.Where(x => x.Type == ProjectEventType.NewInvestor || x.Type == ProjectEventType.OutInvestor).ToList();
                    break;
                //case 5:
                //    events = events.Where(x => x.Type == ProjectEventType.PhaseSwich).ToList();
                //    break;
                case 6:
                    events = events.Where(x => x.Type == ProjectEventType.GetInvesment).ToList();
                    break;
                case 7:
                    events = events.Where(x => x.Type == ProjectEventType.PostNotification).ToList();
                    break;
            }

            return new PagedResultDto<ProjectEventDto>()
            {
                Items = ObjectMapper.Map<List<ProjectEvent>, List<ProjectEventDto>>(events.Skip(input.SkipCount).Take(input.MaxResultCount).OrderByDescending(x => x.EventTime).ToList()),
                TotalCount = events.Count
            };
        }

        public async Task<List<ProjectCalendarEventDto>> GetProjectCalendarEvents(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            var users = await _userRepository.GetListAsync();
            var rs = await _projectCalendarEventRepository.GetListAsync(x => x.ProjectId.Equals(projectId));
            return ObjectMapper.Map<List<ProjectCalendarEvent>, List<ProjectCalendarEventDto>>(rs);
        }

        public async Task AddCalendarEvent(AddProjectCalendarEventDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            input.Start = input.Start.AddHours(7);
            input.End = input.End.AddHours(7);
            await _projectCalendarEventRepository.InsertAsync(new ProjectCalendarEvent()
            {
                ProjectId = input.ProjectId,
                AllDay = input.AllDay,
                AutoDeleteWhenEnd = input.AutoDeleteWhenEnd,
                Start = input.Start,
                End = input.Type == CalendarEventType.TimePeriod ? input.End : input.Start.AddMinutes(30),
                Type = input.Type,
                CreatedById = currentUserId,
                Title = input.Title
            });

        }

        public async Task DeleteCalendarEvent(Guid calendarEventId)
        {
            var calendarEvent = await _projectCalendarEventRepository.GetAsync(calendarEventId);
            var myProjectUser = await _projectUserRepository.FindAsync(x=> x.UserId.Equals(currentUserId) && x.ProjectId.Equals(calendarEvent.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            await _projectCalendarEventRepository.DeleteAsync(calendarEventId);
        }
    }
}
