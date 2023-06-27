using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Startuper.IService;
using FSI.Application.Contracts.User.DTO;
using FSI.Application.Hubs;
using FSI.Common.Enums;
using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.Test;
using FSI.Domain.User;
using FSI.GrpcClient.RecommendationSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

namespace FSI.Application.Startuper
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class StartuperAppService : ApplicationService, IStartuperAppService
    {
        private readonly IStartuperRepository _startuperRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Friend, Guid> _friendRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IRecommendationSystem _recommendationSystem;

        public StartuperAppService(IStartuperRepository startuperRepository, IHttpContextAccessor httpContextAccessor, IRecommendationSystem recommendationSystem, IFileInfomationRepository fileInfomationRepository, IAccountRepository accountRepository, IRepository<Friend, Guid> friendRepository, IRepository<ProjectUser, Guid> projectUserRepository)
        {
            _startuperRepository = startuperRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _recommendationSystem = recommendationSystem;
            _fileInfomationRepository = fileInfomationRepository;
            _accountRepository = accountRepository;
            _friendRepository = friendRepository;
            _projectUserRepository = projectUserRepository;
        }

        public async Task<List<StartuperDto>> GetListAsync()
        {
            var testrs = await _recommendationSystem.Test(this.currentUserId.ToString());
            var rs = await _startuperRepository.GetListAsync();
            return new List<StartuperDto>() { new StartuperDto() { Activity = testrs } };
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
            var rs = await _startuperRepository.UpdateAsync(thisStartuper);
            return ObjectMapper.Map<FSI.Domain.Startuper.Startuper, StartuperDto>(rs);
        }


        public Task<PagedResultDto<StartuperDto>> GetListFounder(GetListFounderDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<StartuperDto>> PostToGetListStartuper(GetListStartuperForProjectDto input)
        {
            var startupers = await _startuperRepository.GetListAsync();
            startupers = startupers.WhereIf(!String.IsNullOrWhiteSpace(input.Filter), x => x.Phone.Equals(input.Filter) ||
                                                                                            x.Name.Contains(input.Filter) ||
                                                                                            x.Describe.Contains(input.Filter) ||
                                                                                            x.Activity.Contains(input.Filter) ||
                                                                                            x.WorkingExperience.Contains(input.Filter))
                                    .WhereIf(input.Fields.Count != 0, x => input.Fields.Contains(x.Field.Value))
                                    .WhereIf(input.Areas.Count != 0, x => input.Areas.Contains(x.Location.Value))
                                    .WhereIf(input.YearOfExps.Count != 0, x => input.YearOfExps.Contains(x.YearOfExp.Value))
                                    .WhereIf(input.AvailableTimes.Count != 0, x => input.AvailableTimes.Contains(x.AvailableTime.Value))
                                    .WhereIf(input.Skills.Count != 0, x => x.Skill.Any(y => input.Skills.Contains(y)))
                                    .WhereIf(input.Personalities.Count != 0, x => x.Personality.Any(y => input.Personalities.Contains(y)))
                                    .ToList();

            var allPatners = await _friendRepository.GetListAsync(x => x.UserAId.Equals(currentUserId) || x.UserBId.Equals(currentUserId));
            var myPatnerIds = allPatners.Where(x => x.IsActive).Select(x =>
            {
                if (x.UserAId.Equals(currentUserId)) return x.UserBId;
                else return x.UserAId;
            });

            var fromMeIds = allPatners.Where(x => x.UserAId.Equals(currentUserId) && !x.IsActive).Select(x => x.UserBId);
            var toMeIds = allPatners.Where(x => x.UserBId.Equals(currentUserId) && !x.IsActive).Select(x => x.UserAId);

            var allIds = myPatnerIds.Concat(fromMeIds).Concat(toMeIds);

            if (input.Mode.Equals(GuidStartuperMode.UuidStartuperModeNew))
            {
                startupers = startupers.Where(x=> !allIds.Contains(x.Id)).ToList();
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
                startupers = startupers.Where(x => fromMeIds.Contains(x.Id)).ToList();
            }
            else
            {
                var projectUserIds = (await _projectUserRepository.GetListAsync(x=> x.ProjectId.Equals(input.Mode))).Select(x => x.UserId);
                startupers = startupers.Where(x => !projectUserIds.Contains(x.Id)).ToList();
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

        public async Task UpdateBaseInfo(UpdateBaseInfoDto input)
        {
            var myUserInfo = await _startuperRepository.GetAsync(currentUserId);
            var acc = await _accountRepository.GetAsync(myUserInfo.AccountId);

            myUserInfo.Name = input.Name;
            myUserInfo.Phone = input.PhoneNumber ?? myUserInfo.Phone;
            myUserInfo.DateOfBirth = input.DateOfBirth;
            myUserInfo.IdentityCard = input.IdentityCard;
            myUserInfo.Location = input.Location;
            myUserInfo.WorkingPlace = input.WorkingPlace;

            acc.Email = input.Email ?? acc.Email;
            acc.PhoneNumber = input.PhoneNumber ?? acc.PhoneNumber;
            await _startuperRepository.UpdateAsync(myUserInfo);
            await _accountRepository.UpdateAsync(acc);
        }
    }
}
