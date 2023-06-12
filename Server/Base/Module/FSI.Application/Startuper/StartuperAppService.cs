using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Startuper.IService;
using FSI.Application.Hubs;
using FSI.Domain.File;
using FSI.Domain.Startuper;
using FSI.Domain.Test;
using FSI.Domain.User;
using FSI.GrpcClient.RecommendationSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Startuper
{
    [Authorize(Roles = "Startuper")]
    public class StartuperAppService : ApplicationService, IStartuperAppService
    {
        private readonly IStartuperRepository _startuperRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IRecommendationSystem _recommendationSystem;

        public StartuperAppService(IStartuperRepository startuperRepository, IHttpContextAccessor httpContextAccessor, IRecommendationSystem recommendationSystem, IFileInfomationRepository fileInfomationRepository)
        {
            _startuperRepository = startuperRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _recommendationSystem = recommendationSystem;
            _fileInfomationRepository = fileInfomationRepository;
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
            thisStartuper.YearOfExp= input.YearOfExp;
            thisStartuper.AvailableTime = input.AvailableTime;
            thisStartuper.WorkingExperience = input.WorkingExperience;
            thisStartuper.IsNewProfile = false;
            var rs = await _startuperRepository.UpdateAsync(thisStartuper);
            return ObjectMapper.Map<FSI.Domain.Startuper.Startuper, StartuperDto>(rs);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _startuperRepository.DeleteAsync(id);
        }

        public Task<PagedResultDto<StartuperDto>> GetListFounder(GetListFounderDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<StartuperDto>> GetListStartuperForProject(GetListStartuperForProjectDto input)
        {
            throw new NotImplementedException();
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
                Size = (int)file.Length
            });

            var myInfo = await _startuperRepository.GetAsync(this.currentUserId);
            myInfo.AvatarUrl = fileUrl;
            await _startuperRepository.UpdateAsync(myInfo);
        }
    }
}
