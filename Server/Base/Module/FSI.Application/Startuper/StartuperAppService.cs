using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Startuper.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Application.EventHandle;
using FSI.Application.Hubs;
using FSI.Common.Enums;
using FSI.Common.ETO;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Investor;
using FSI.Domain.MatrixRating;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.Test;
using FSI.Domain.User;
using FSI.GrpcClient.RecommendationSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Startuper
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class StartuperAppService : ApplicationService, IStartuperAppService
    {
        private readonly IStartuperRepository _startuperRepository;
        private readonly IRepository<StartuperSimilarity, Guid> _startuperSimilarityRepository;
        private readonly IInvestorRepository _investorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Friend, Guid> _friendRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IRepository<ProjectRequestStartuperInfo, Guid> _projectRequestStartuperInfoRepository;
        private readonly IDistributedCache<List<ProjectSimilarStartuper>> _projectSimilarStartuperCache;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IConfiguration Configuration;

        private readonly IDistributedEventBus _distributedEventBus;

        private readonly IRecommendationSystem _recommendationSystem;

        private readonly IBlobContainer _blobContainer;

        public StartuperAppService(IStartuperRepository startuperRepository, IHttpContextAccessor httpContextAccessor, IRecommendationSystem recommendationSystem, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository, IRepository<Friend, Guid> friendRepository, IRepository<ProjectUser, Guid> projectUserRepository, IProjectRepository projectRepository, IInvestorRepository investorRepository, IUserRootRepository userRepository, IDistributedEventBus distributedEventBus, IRepository<StartuperSimilarity, Guid> startuperSimilarityRepository, IRepository<ProjectRequestStartuperInfo, Guid> projectRequestStartuperInfoRepository, IDistributedCache<List<ProjectSimilarStartuper>> projectSimilarStartuperCache, IConfiguration configuration, IBlobContainer blobContainer = null)
        {
            _startuperRepository = startuperRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _recommendationSystem = recommendationSystem;
            _fileInfomationRepository = fileInfomationRepository;
            _accountRepository = accountRepository;
            _friendRepository = friendRepository;
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
            _investorRepository = investorRepository;
            _userRepository = userRepository;
            _distributedEventBus = distributedEventBus;
            _startuperSimilarityRepository = startuperSimilarityRepository;
            _projectRequestStartuperInfoRepository = projectRequestStartuperInfoRepository;
            _projectSimilarStartuperCache = projectSimilarStartuperCache;
            Configuration = configuration;
            _blobContainer = blobContainer;
        }

        public async Task<StartuperDto> InsertStartuperAsync(CreateStartuperDto input)
        {
            var thisStartuper = await _startuperRepository.GetAsync(this.currentUserId);
            thisStartuper.Speciality = input.Speciality;
            thisStartuper.Field = input.Field;
            thisStartuper.Activity = input.Activity;
            thisStartuper.Personality = input.Personality;
            thisStartuper.CertificateAndAward = input.CertificateAndAward;
            thisStartuper.Skill = input.Skill;
            thisStartuper.hasProject = input.hasProject;
            thisStartuper.Describe = input.Describe;
            thisStartuper.YearOfExp = input.YearOfExp;
            thisStartuper.AvailableTime = input.AvailableTime;
            thisStartuper.WorkingExperience = input.WorkingExperience;
            thisStartuper.IsNewProfile = false;
            thisStartuper.Collab = input.Collab;
            thisStartuper.RequestPersonality = input.RequestPersonality;
            thisStartuper.RequestSkill = input.RequestSkill;
            thisStartuper.hasIdea = false;
            var rs = await _startuperRepository.UpdateAsync(thisStartuper);

            await _distributedEventBus.PublishAsync(new UpdateStartuperInfoEto()
            {
                StartuperId = rs.Id
            });
            return ObjectMapper.Map<FSI.Domain.Startuper.Startuper, StartuperDto>(rs);
        }

        public async Task<PagedResultDto<StartuperDto>> PostToGetListStartuper(GetListStartuperForStartuperDto input)
        {
            var startupersQrb = await _startuperRepository.GetQueryableAsync();
            var startupers = startupersQrb.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.Phone.Equals(input.Filter) ||
                                                                                            x.Name.Contains(input.Filter) ||
                                                                                            x.Describe.Contains(input.Filter) ||
                                                                                            x.Activity.Contains(input.Filter) ||
                                                                                            x.WorkingExperience.Contains(input.Filter) ||
                                                                                            x.StudentId.Equals(input.Filter))
                                    .WhereIf(input.Specializies.Count != 0, x => input.Specializies.Contains(x.Field.Value))
                                    .WhereIf(input.Areas.Count != 0, x => input.Areas.Contains(x.Location))
                                    .WhereIf(input.YearOfExps.Count != 0, x => input.YearOfExps.Contains(x.YearOfExp.Value))
                                    .WhereIf(input.AvailableTimes.Count != 0, x => input.AvailableTimes.Contains(x.AvailableTime.Value))
                                    .WhereIf(input.Skills.Count != 0, x => x.Skill.Any(y => input.Skills.Contains(y)))
                                    .WhereIf(input.Personalities.Count != 0, x => x.Personality.Any(y => input.Personalities.Contains(y)))
                                    .Where(x => !x.Id.Equals(currentUserId))
                                    .ToList();

            if (input.IsStudent.Value)
            {
                startupers = startupers.Where(x => x.Job == 1)
                                        .WhereIf(!String.IsNullOrEmpty(input.University), x => x.University != null && x.University.Equals(input.University))
                                        .WhereIf(!String.IsNullOrEmpty(input.UniversitySpecialized), x => x.UniversitySpecialized != null && x.UniversitySpecialized.Equals(input.UniversitySpecialized))
                                        .WhereIf(!String.IsNullOrEmpty(input.StudentId), x => x.StudentId != null && x.StudentId.Contains(input.StudentId))
                            .ToList();
            }

            var allPatners = await _friendRepository.GetListAsync(x => x.UserAId.Equals(currentUserId) || x.UserBId.Equals(currentUserId));
            var myPatnerIds = allPatners.Where(x => x.IsActive).Select(x =>
            {
                if (x.UserAId.Equals(currentUserId)) return x.UserBId;
                else return x.UserAId;
            }).ToList();

            var fromMeIds = allPatners.Where(x => x.UserAId.Equals(currentUserId) && !x.IsActive).Select(x => x.UserBId).ToList();
            var toMeIds = allPatners.Where(x => x.UserBId.Equals(currentUserId) && !x.IsActive).Select(x => x.UserAId).ToList();

            var allIds = myPatnerIds.Concat(fromMeIds).Concat(toMeIds).ToList();

            if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeNew))
            {
                startupers = startupers.Where(x => !allIds.Contains(x.Id)).ToList();
            }
            else if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeOfMe))
            {
                startupers = startupers.Where(x => myPatnerIds.Contains(x.Id)).ToList();
            }
            else if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeFromMe))
            {
                startupers = startupers.Where(x => fromMeIds.Contains(x.Id)).ToList();
            }
            else if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeToMe))
            {
                startupers = startupers.Where(x => toMeIds.Contains(x.Id)).ToList();
            }
            else
            {
                var projectUserIds = (await _projectUserRepository.GetListAsync(x => x.ProjectId.Equals(input.Mode))).Select(x => x.UserId);
                startupers = startupers.Where(x => !projectUserIds.Contains(x.Id)).ToList();

                var similarities = await _projectSimilarStartuperCache.GetAsync(input.Mode.ToString());

                if (similarities == null)
                {
                    await _distributedEventBus.PublishAsync(new UpdateProjectRequestStartuperInfoEto()
                    {
                        ProjectId = input.Mode.Value
                    });

                    var rq = await _projectRequestStartuperInfoRepository.FindAsync(x => x.ProjectId.Equals(input.Mode));
                    similarities = rq?.Similarities;
                }

                if (similarities != null)
                {
                    var query = from startuper in startupers
                                join similarity in similarities
                                on startuper.Id equals similarity.StartuperId
                                into gj
                                from subSimilarity in gj.DefaultIfEmpty()
                                select new
                                {
                                    Startuper = startuper,
                                    Similarity = subSimilarity?.Similarity ?? 0
                                };

                    var startupersIncludeSimilarity = query.OrderByDescending(x => x.Similarity).ToList();
                    startupers = startupersIncludeSimilarity.Select(x =>
                    {
                        x.Startuper.SetProperty("similarity", x.Similarity);
                        return x.Startuper;
                    }).ToList();
                }
            }

            // join với startuperSililarity để order theo similarity với người dùng hiện tại
            if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeNew) ||
                input.Mode.Equals(GuidStartuperMode.UuidStartuperModeFromMe) ||
                input.Mode.Equals(GuidStartuperMode.UuidStartuperModeToMe) ||
                input.Mode.Equals(GuidStartuperMode.UuidStartuperModeOfMe))
            {
                var startuperSimilarities = await _startuperSimilarityRepository.GetListAsync(x => x.UserId.Equals(currentUserId));
                var query = from startuper in startupers
                            join similarity in startuperSimilarities
                            on startuper.Id equals similarity.TargetId
                            into gj
                            from subSimilarity in gj.DefaultIfEmpty()
                            select new
                            {
                                Startuper = startuper,
                                Similarity = subSimilarity?.Similarity ?? 0
                            };

                var startupersIncludeSimilarity = query.OrderByDescending(x => x.Similarity).ToList();
                startupers = startupersIncludeSimilarity.Select(x =>
                {
                    x.Startuper.SetProperty("similarity", x.Similarity);
                    return x.Startuper;
                }).ToList();
            }

            var startuperPageds = startupers.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<StartuperDto>()
            {
                Items = ObjectMapper.Map<List<FSI.Domain.Startuper.Startuper>, List<StartuperDto>>(startuperPageds),
                TotalCount = startupers.Count
            };
        }

        public async Task<bool> GetCheckIsNewProfile()
        {
            var startuper = await _startuperRepository.GetAsync(this.currentUserId);
            return (bool)startuper.IsNewProfile;
        }

        public async Task UploadAvatar()
        {
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

            var myInfo = await _startuperRepository.GetAsync(this.currentUserId);
            myInfo.AvatarUrl = fileUrl;
            await _startuperRepository.UpdateAsync(myInfo);
        }

        public async Task<StartuperDto> GetMyInfoAsync()
        {
            var myInfo = await _startuperRepository.GetAsync(currentUserId);
            var acc = await _accountRepository.GetAsync(myInfo.AccountId);
            myInfo.SetProperty("phoneNumber", acc.PhoneNumber);
            myInfo.SetProperty("email", acc.Email);
            return ObjectMapper.Map<Domain.Startuper.Startuper, StartuperDto>(myInfo);
        }

        public async Task PostUpdateBaseInfo(UpdateBaseInfoDto input)
        {
            var myUserInfo = await _startuperRepository.GetAsync(currentUserId);
            var acc = await _accountRepository.GetAsync(myUserInfo.AccountId);

            myUserInfo.Name = input.Name;
            myUserInfo.Phone = input.PhoneNumber ?? myUserInfo.Phone;
            myUserInfo.DateOfBirth = input.DateOfBirth;
            myUserInfo.Location = input.Location;
            myUserInfo.WorkingPlace = input.WorkingPlace;
            myUserInfo.Gender = input.Gender;
            myUserInfo.Job = input.Job;
            myUserInfo.University = input.University;
            myUserInfo.UniversitySpecialized = input.UniversitySpecialized;
            myUserInfo.StudentId = input.StudentId;

            acc.Email = input.Email ?? acc.Email;
            acc.PhoneNumber = input.PhoneNumber ?? acc.PhoneNumber;
            await _startuperRepository.UpdateAsync(myUserInfo);
            await _accountRepository.UpdateAsync(acc);

            await _distributedEventBus.PublishAsync(new UpdateStartuperInfoEto()
            {
                StartuperId = currentUserId
            });
        }

        public async Task<List<ProjectUserDto>> GetMyProjects()
        {
            var projects = await _projectRepository.GetListAsync();
            var myProjectUsers = await _projectUserRepository.GetListAsync(x => x.IsActive && x.UserId.Equals(currentUserId));
            return ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(myProjectUsers);
        }

        public async Task RequestFriendToOrtherStartuper(Guid targetId)
        {
            var friend = await _friendRepository.FindAsync(x => (x.UserAId.Equals(currentUserId) && x.UserBId.Equals(targetId)) ||
                                                                (x.UserAId.Equals(targetId) && x.UserBId.Equals(currentUserId)));

            if (friend == null)
            {
                await _friendRepository.InsertAsync(new Friend()
                {
                    UserAId = currentUserId,
                    UserBId = targetId,
                    IsActive = false
                });
            }
            else
                throw new UserFriendlyException(message: "Đã kết nối hoặc đã gửi lời mời kết nối!");
        }

        public async Task AcceptRequestFriendFromOrtherStartuper(Guid targetId)
        {
            var friend = await _friendRepository.GetAsync(x => x.UserAId.Equals(targetId) && x.UserBId.Equals(currentUserId));

            if (friend.IsActive)
                throw new UserFriendlyException(message: "Request đã được chấp nhận từ trước!");

            friend.IsActive = true;
            await _friendRepository.UpdateAsync(friend);
        }

        public async Task CancelRequestToOrtherStartuper(Guid targetId)
        {
            var friend = await _friendRepository.GetAsync(x => x.UserAId.Equals(currentUserId) && x.UserBId.Equals(targetId));

            if (friend.IsActive)
                throw new UserFriendlyException(message: "Request đã được chấp nhận, không thể hủy bỏ!");

            await _friendRepository.DeleteAsync(friend);
        }

        public async Task<UserDetailDto> GetUserDetail(Guid userId)
        {
            var startuperInfo = await _startuperRepository.FindAsync(userId);
            var investorInfo = await _investorRepository.FindAsync(userId);
            var projects = await _projectRepository.GetListAsync();
            var projectUsers = await _projectUserRepository.GetListAsync(x => x.UserId.Equals(userId) && x.IsActive);

            var friend = await _friendRepository.FindAsync(x => (x.UserAId.Equals(currentUserId) && x.UserBId.Equals(userId)) ||
                                                                (x.UserAId.Equals(userId) && x.UserBId.Equals(currentUserId)));

            int friendStatus;
            if (friend == null)
            {
                var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                if (role == "Admin")
                    friendStatus = -1;
                else
                    friendStatus = 0;
            }
            else
            {
                if (friend.IsActive)
                {
                    friendStatus = 1;
                }
                else
                {
                    if (friend.UserAId.Equals(currentUserId))
                    {
                        friendStatus = 2;
                    }
                    else
                    {
                        friendStatus = 3;
                    }
                }
            }

            return new UserDetailDto()
            {
                InvestorInfo = ObjectMapper.Map<FSI.Domain.Investor.Investor, InvestorDto>(investorInfo),
                StartuperInfo = ObjectMapper.Map<FSI.Domain.Startuper.Startuper, StartuperDto>(startuperInfo),
                ProjectAsInvestor = ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(projectUsers.Where(x => x.Role == RoleInProject.Investor).ToList()),
                ProjectAsStartuper = ObjectMapper.Map<List<ProjectUser>, List<ProjectUserDto>>(projectUsers.Where(x => x.Role != RoleInProject.Investor).ToList()),
                FriendStatus = friendStatus
            };
        }

        public async Task<UserRootDto> GetUserByUsername(string username)
        {
            var acc = await _accountRepository.GetAsync(x => x.PhoneNumber.Equals(username) || x.Email.Equals(username));

            var user = await _userRepository.GetAsync(x => x.AccountId.Equals(acc.Id));

            return ObjectMapper.Map<UserRoot, UserRootDto>(user);
        }

    }
}
