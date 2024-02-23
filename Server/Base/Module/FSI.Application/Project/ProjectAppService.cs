using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.DTO.Hiring;
using FSI.Application.Contracts.Project.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Application.EventHandle;
using FSI.Common.Enums;
using FSI.Common.ETO;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.MatrixRating;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Pipelines.Sockets.Unofficial.Arenas;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace FSI.Application.Project
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectSimilarity, Guid> _projectSimilarityRepository;
        private readonly IRepository<StartuperSimilarity, Guid> _startuperSimilarityRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IRepository<ProjectFile, Guid> _projectFileRepository;
        private readonly IRepository<ProjectEvent, Guid> _projectEventRepository;
        private readonly IRepository<ProjectWork, Guid> _projectWorkRepository;
        private readonly IRepository<ProjectCalendarEvent, Guid> _projectCalendarEventRepository;
        private readonly IRepository<ProjectRequestStartuperInfo, Guid> _projectRequestStartuperInfoRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<UserProjectRating, Guid> _userProjectRatingRepository;
        private readonly IDistributedCache<List<PredictRatingProject>> _predictRatingProjectForStartuperIdCache;
        private readonly IDistributedCache<string> _testCache;
        private readonly IConfiguration Configuration;

        private readonly IDistributedEventBus _distributedEventBus;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IBlobContainer _blobContainer;

        public ProjectAppService(IProjectRepository projectRepository, IRepository<ProjectUser, Guid> projectUserRepository, IHttpContextAccessor httpContextAccessor, IUserRootRepository userRepository, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository, IRepository<ProjectFile, Guid> projectFileRepository, IRepository<ProjectEvent, Guid> projectEventRepository, IRepository<ProjectCalendarEvent, Guid> projectCalendarEventRepository, IRepository<ProjectWork, Guid> projectWorkRepository, IDistributedEventBus distributedEventBus, IRepository<ProjectSimilarity, Guid> projectSimilarityRepository, IRepository<UserProjectRating, Guid> userProjectRatingRepository, IRepository<StartuperSimilarity, Guid> startuperSimilarityRepository, IRepository<ProjectRequestStartuperInfo, Guid> projectRequestStartuperInfoRepository, IDistributedCache<List<PredictRatingProject>> predictRatingProjectForStartuperIdCache, IDistributedCache<string> testCache, IConfiguration configuration, IBlobContainer blobContainer = null)
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
            _projectWorkRepository = projectWorkRepository;
            _distributedEventBus = distributedEventBus;
            _projectSimilarityRepository = projectSimilarityRepository;
            _userProjectRatingRepository = userProjectRatingRepository;
            _startuperSimilarityRepository = startuperSimilarityRepository;
            _projectRequestStartuperInfoRepository = projectRequestStartuperInfoRepository;
            _predictRatingProjectForStartuperIdCache = predictRatingProjectForStartuperIdCache;
            _testCache = testCache;
            Configuration = configuration;
            _blobContainer = blobContainer;
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
                FounderId = this.currentUserId,
                IsActive = false
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

            await _distributedEventBus.PublishAsync(new UpdateProjectInfoEto()
            {
                ProjectId = project.Id
            });

            var rs = ObjectMapper.Map<FSI.Domain.Project.Project, ProjectDto>(project);

            await _userProjectRatingRepository.InsertAsync(new UserProjectRating()
            {
                UserId = currentUserId,
                ProjectId = rs.Id,
                Rating = 3
            });

            return rs;
        }

        public async Task<ProjectDto> PostUpdateProjectAsync(CreateProjectDto input)
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
            project.Fields = input.Fields;

            await _distributedEventBus.PublishAsync(new UpdateProjectInfoEto()
            {
                ProjectId = project.Id
            });

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

            var rating = await _userProjectRatingRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));
            if (rating == null)
            {
                await _userProjectRatingRepository.InsertAsync(new UserProjectRating()
                {
                    UserId = currentUserId,
                    ProjectId = projectId,
                    Rating = 1
                });
            }
            else
            {
                rating.Rating = 1;
                await _userProjectRatingRepository.UpdateAsync(rating);
            }

            return rs;
        }

        public async Task<List<ProjectDto>> GetTopProjectSimilarByProjectId(Guid projectId)
        {
            var projects = await _projectRepository.GetListAsync(x => !x.Id.Equals(projectId) && x.IsActive.Value);

            var projectOfMes = await _projectUserRepository.GetListAsync(x => x.UserId.Equals(currentUserId));
            var projectOfMeIds = projectOfMes.Select(x => x.ProjectId);

            projects = projects.Where(x => !projectOfMeIds.Contains(x.Id)).ToList();

            var projectSimilarities = await _projectSimilarityRepository.GetListAsync(x => x.ProjectId.Equals(projectId));

            var query = from project in projects
                        join similarity in projectSimilarities
                        on project.Id equals similarity.ProjectTargetId
                        into gj
                        from subSimilarity in gj.DefaultIfEmpty()
                        select new
                        {
                            Project = project,
                            Similarity = subSimilarity?.Similarity ?? 1
                        };

            var projectsIncludeSimilarity = query.ToList();
            projectsIncludeSimilarity = projectsIncludeSimilarity.OrderByDescending(x => x.Similarity).ToList();

            var projectsOrderBySimilarity = projectsIncludeSimilarity.Select(x =>
            {
                x.Project.SetProperty("similarity", x.Similarity);
                return x.Project;
            }).Skip(0).Take(10).ToList();

            return ObjectMapper.Map<List<FSI.Domain.Project.Project>, List<ProjectDto>>(projectsOrderBySimilarity);

        }

        public async Task<PagedResultDto<ProjectDto>> PostToGetListProjectForStartuper(GetListProjectForStartuperDto input)
        {
            var projects = await _projectRepository.GetListAsync();

            projects = projects.Where(x => x.IsActive.Value)
                                .WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.ProjectName.Contains(input.Filter) || x.Description.Contains(input.Filter))
                                .WhereIf(input.Areas.Count != 0, x => input.Areas.Contains(x.Area.Value))
                                .WhereIf(input.Stages.Count != 0, x => input.Stages.Contains(x.Stage.Value))
                                .WhereIf(input.Scales.Count != 0, x => input.Scales.Contains(x.Scale.Value))
                                .WhereIf(input.Fields.Count != 0, x => x.Fields.Any(y => input.Fields.Contains(y)))
                                .WhereIf(input.WorkingForm.HasValue, x=> x.WorkingForm == input.WorkingForm)
                                .WhereIf(input.IsProfit.HasValue, x=> x.IsProfit == input.IsProfit).ToList();

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

            //switch (input.Sorting)
            //{
            //    case "":
            //        break;
            //}

            if (input.RelationWithProject == RelationWithProject.NotMemberOfProject)
            {

                var ratings = await _predictRatingProjectForStartuperIdCache.GetAsync(currentUserId.ToString());

                if (ratings == null)
                {
                    var allUserRating = await _userProjectRatingRepository.GetListAsync();
                    var mySimilars = await _startuperSimilarityRepository.GetListAsync(x => x.UserId.Equals(currentUserId));
                    var projectIds = projects.Select(x => x.Id).ToList();
                    ratings = RatingPredictClass.PredictRating(allUserRating, mySimilars, projectIds, 10);
                    await _predictRatingProjectForStartuperIdCache.SetAsync(currentUserId.ToString(), ratings, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                    });
                }

                var query = from project in projects
                            join rating in ratings
                            on project.Id equals rating.ProjectId
                            into gj
                            from subRating in gj.DefaultIfEmpty()
                            select new
                            {
                                Project = project,
                                Rating = subRating?.PredictRating ?? 0f
                            };

                projects = query.OrderByDescending(x => x.Rating).Select(x =>
                {
                    x.Project.SetProperty("predictRating", x.Rating);
                    return x.Project;
                }).ToList();
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

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                await _blobContainer.SaveAsync(fileName, stream.ToArray(), overrideExisting: true);
            }

            var fileUrl = "https://fsiconnectedapi.azurewebsites.net/image/" + fileName;

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

            var rating = await _userProjectRatingRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));
            if (rating == null)
            {
                await _userProjectRatingRepository.InsertAsync(new UserProjectRating()
                {
                    UserId = currentUserId,
                    ProjectId = projectId,
                    Rating = 2
                });
            }
            else
            {
                rating.Rating = 2;
                await _userProjectRatingRepository.UpdateAsync(rating);
            }
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

                    var rating = await _userProjectRatingRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));
                    if (rating == null)
                    {
                        await _userProjectRatingRepository.InsertAsync(new UserProjectRating()
                        {
                            UserId = currentUserId,
                            ProjectId = projectId,
                            Rating = 3
                        });
                    }
                    else
                    {
                        rating.Rating = 3;
                        await _userProjectRatingRepository.UpdateAsync(rating);
                    }
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

            var rating = await _userProjectRatingRepository.FindAsync(x => x.UserId.Equals(userId) && x.ProjectId.Equals(projectId));
            if (rating == null)
            {
                await _userProjectRatingRepository.InsertAsync(new UserProjectRating()
                {
                    UserId = userId,
                    ProjectId = projectId,
                    Rating = 3
                });
            }
            else
            {
                rating.Rating = 3;
                await _userProjectRatingRepository.UpdateAsync(rating);
            }
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
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    await _blobContainer.SaveAsync(fileName, stream.ToArray(), overrideExisting: true);
                }

                var fileUrl = "https://fsiconnectedapi.azurewebsites.net/image/" + fileName;
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
                ProjectId = input.ProjectId,
                IsPublic = input.IsPublic
            });
        }

        public async Task<PagedResultDto<ProjectEventDto>> PostToGetEventsOfProject(GetProjectEventsDto input)
        {
            var users = await _userRepository.GetListAsync();
            var events = await _projectEventRepository.GetListAsync(x => x.ProjectId.Equals(input.ProjectId));

            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
            {
                events = events.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.Content.Contains(input.Filter))
                                .Where(x => x.Type != ProjectEventType.PostNotification || x.IsPublic.Value).ToList();
            }
            else
            {
                events = events.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.Content.Contains(input.Filter)).ToList();
            }

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
            {
                var users = await _userRepository.GetListAsync();
                var rs = await _projectCalendarEventRepository.GetListAsync(x => x.ProjectId.Equals(projectId) && x.IsPublic);
                return ObjectMapper.Map<List<ProjectCalendarEvent>, List<ProjectCalendarEventDto>>(rs);
            }
            else
            {
                var users = await _userRepository.GetListAsync();
                var rs = await _projectCalendarEventRepository.GetListAsync(x => x.ProjectId.Equals(projectId));
                return ObjectMapper.Map<List<ProjectCalendarEvent>, List<ProjectCalendarEventDto>>(rs);
            }
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
                Title = input.Title,
                IsPublic = input.IsPublic
            });

        }

        public async Task DeleteCalendarEvent(Guid calendarEventId)
        {
            var calendarEvent = await _projectCalendarEventRepository.GetAsync(calendarEventId);
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(calendarEvent.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            await _projectCalendarEventRepository.DeleteAsync(calendarEventId);
        }

        public async Task<ProjectWorkDto> AddWork(AddProjectWorkDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var work = await _projectWorkRepository.InsertAsync(new ProjectWork()
            {
                ProjectId = input.ProjectId,
                AssigneeId = input.AssigneeId,
                AssignorId = currentUserId,
                Title = input.Title,
                Description = input.Description,
                Deadline = input.Deadline,
                FileIds = input.FileIds,
                Status = WorkStatus.New
            });

            return ObjectMapper.Map<ProjectWork, ProjectWorkDto>(work);
        }

        public async Task ChangeWorkStatus(Guid workId, WorkStatus newStatus)
        {
            var work = await _projectWorkRepository.GetAsync(workId);
            if (!work.AssigneeId.Equals(currentUserId) && !work.AssignorId.Equals(currentUserId))
                throw new UserFriendlyException(message: "Bạn không thể thay đổi trạng thái cho công việc này!");

            work.Status = newStatus;
            await _projectWorkRepository.UpdateAsync(work);
        }

        public async Task<List<ProjectWorkDto>> GetProjectWorks(Guid projectId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(this.currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");
            var users = await _userRepository.GetListAsync();

            var works = await _projectWorkRepository.GetListAsync(x => x.ProjectId.Equals(projectId));

            return ObjectMapper.Map<List<ProjectWork>, List<ProjectWorkDto>>(works);
        }

        public async Task<ProjectRequestStartuperInfoDto> GetProjectRequestStartuperInfo(Guid projectId)
        {
            var rs = await _projectRequestStartuperInfoRepository.FindAsync(x => x.ProjectId.Equals(projectId));
            if (rs == null)
                return null;
            else return ObjectMapper.Map<ProjectRequestStartuperInfo, ProjectRequestStartuperInfoDto>(rs);
        }

        public async Task PostUpdateProjectRequestStartuperInfo(ProjectRequestStartuperInfoDto input)
        {
            var rqInfo = await _projectRequestStartuperInfoRepository.FindAsync(x => x.ProjectId.Equals(input.ProjectId));
            if (rqInfo == null)
            {
                await _projectRequestStartuperInfoRepository.InsertAsync(ObjectMapper.Map<ProjectRequestStartuperInfoDto, ProjectRequestStartuperInfo>(input));
            }
            else
            {
                rqInfo.Describe = input.Describe;
                rqInfo.Locations = input.Locations;
                rqInfo.Fields = input.Fields;
                rqInfo.Jobs = input.Jobs;
                rqInfo.Personalities = input.Personalities;
                rqInfo.Skills = input.Skills;
                rqInfo.Activity = input.Activity;
                rqInfo.AvailableTimes = input.AvailableTimes;
                rqInfo.YearOfExps = input.YearOfExps;
                rqInfo.WorkingPlace = input.WorkingPlace;
                rqInfo.WorkingExperience = input.WorkingExperience;
                rqInfo.Speciality = input.Speciality;
                rqInfo.CertificateAndAward = input.CertificateAndAward;
            }

            await _distributedEventBus.PublishAsync(new UpdateProjectRequestStartuperInfoEto()
            {
                ProjectId = input.ProjectId,
            });
        }

        public async Task TestAzureRedis(string value)
        {
            await _testCache.SetAsync("adu", value, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        }

        public async Task<List<ProjectHiringDto>> GetProjectHirings(Guid projectId)
        {
            var project = await _projectRepository.GetProjectWithHirings(projectId);

            return ObjectMapper.Map<List<ProjectHiring>, List<ProjectHiringDto>>(project.Hirings);
        }

        public async Task<ProjectHiringDto> GetProjectHiring(Guid projectId, Guid hiringId)
        {
            var project = await _projectRepository.GetProjectWithHirings(projectId);
            var hiring = project.Hirings.FirstOrDefault(x => x.Id == hiringId);

            return ObjectMapper.Map<ProjectHiring, ProjectHiringDto>(hiring);
        }


        public async Task CreateProjectHiring(CreateOrUpdateProjectHiringDto input)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var hiring = ObjectMapper.Map<CreateOrUpdateProjectHiringDto, ProjectHiring>(input);
            var project = await _projectRepository.GetAsync(input.ProjectId);
            if (project.Hirings == null)
            {
                project.Hirings = new List<ProjectHiring> { hiring };
            }
            else
                project.Hirings.Add(hiring);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task UpdateProjectHiring(CreateOrUpdateProjectHiringDto input)
        {

            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(input.ProjectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var project = await _projectRepository.GetProjectWithHirings(input.ProjectId);

            var hiring = project.Hirings.FirstOrDefault(x => x.Id == input.Id);

            hiring.Title = input.Title;
            hiring.Quantity = input.Quantity;
            hiring.Specialize = input.Specialize;
            hiring.WorkingForm = input.WorkingForm;
            hiring.Location = input.Location;
            hiring.WorkingAddress = input.WorkingAddress;
            hiring.WorkingTimes = input.WorkingTimes;
            hiring.Description = input.Description;
            hiring.YearOfExps = input.YearOfExps;
            hiring.Degree = input.Degree;
            hiring.Skills = input.Skills;
            hiring.Personalities = input.Personalities;
            hiring.OtherRequest = input.OtherRequest;
            hiring.OtherDetail = input.OtherDetail;
            hiring.Duration = input.Duration;

            hiring.IncodeMode = input.IncodeMode;
            hiring.IncomeFrom = input.IncomeFrom;
            hiring.IncomeTo = input.IncomeTo;
            hiring.IncomeRange = input.IncomeRange;

            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectHiring(Guid projectId, Guid hiringId)
        {
            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectId));
            if (myProjectUser == null || !myProjectUser.IsActive)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var project = await _projectRepository.GetProjectWithHirings(projectId);
            var hiring = project.Hirings.FirstOrDefault(x => x.Id == hiringId);
            project.Hirings.Remove(hiring);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task<string> GetProjectCanvasModel(Guid projectId)
        {
            var project = await _projectRepository.GetAsync(projectId);
            return project.TheLeanCanvasBusinessModel;
        }

        public async Task UpdateProjectCanvasModel(UpdateCanvasModelDto input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            if (project.FounderId != currentUserId)
                throw new UserFriendlyException("Chỉ founder dự án mới có quyền cập nhật!");

            project.TheLeanCanvasBusinessModel = input.Model;
            await _projectRepository.UpdateAsync(project);
        }

    }
}
