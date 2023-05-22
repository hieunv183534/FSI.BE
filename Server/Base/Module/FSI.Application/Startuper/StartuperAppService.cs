using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Startuper.IService;
using FSI.Application.Hubs;
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
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Startuper
{
    [Authorize(Roles = "Startuper")]
    public class StartuperAppService : ApplicationService, IStartuperAppService
    {
        private readonly IStartuperRepository _startuperRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IRecommendationSystem _recommendationSystem;

        public StartuperAppService(IStartuperRepository startuperRepository, IHttpContextAccessor httpContextAccessor, IRecommendationSystem recommendationSystem)
        {
            _startuperRepository = startuperRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _recommendationSystem = recommendationSystem;
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
            thisStartuper.Activity = input.Activity;
            thisStartuper.Personality = input.Personality;
            thisStartuper.Award = input.Award;
            thisStartuper.FavoriteField = input.FavoriteField;
            thisStartuper.Certificate = input.Certificate;
            thisStartuper.Skill = input.Skill;
            thisStartuper.WorkingExperience = input.WorkingExperience;
            var rs = await _startuperRepository.UpdateAsync(thisStartuper);
            return ObjectMapper.Map<FSI.Domain.Startuper.Startuper, StartuperDto>(rs);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _startuperRepository.DeleteAsync(id);
        }
    }
}
