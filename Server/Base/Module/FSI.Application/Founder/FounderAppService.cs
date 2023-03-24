using FSI.Application.Contracts.Founder.DTO;
using FSI.Application.Contracts.Founder.IService;
using FSI.Application.Hubs;
using FSI.Domain.Founder;
using FSI.Domain.Test;
using FSI.Domain.User;
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

namespace FSI.Application.Founder
{
    [Authorize]
    [Authorize(Roles = "Founder")]
    public class FounderAppService : ApplicationService, IFounderAppService
    {
        private readonly IFounderRepository _founderRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;
        public FounderAppService(IFounderRepository founderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _founderRepository = founderRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public async Task<List<FounderDto>> GetListAsync()
        {
            var rs = await _founderRepository.GetListAsync();
            return ObjectMapper.Map<List<FSI.Domain.Founder.Founder>, List<FounderDto>>(rs);
        }

        public async Task<FounderDto> InsertFounderAsync(CreateFounderDto input)
        {
            var thisFounder = await _founderRepository.GetAsync(this.currentUserId);
            thisFounder.Speciality = input.Speciality;
            thisFounder.Activity = input.Activity;
            thisFounder.Personality = input.Personality;
            thisFounder.Award = input.Award;
            thisFounder.FavoriteField = input.FavoriteField;
            thisFounder.Certificate = input.Certificate;
            thisFounder.Skill = input.Skill;
            thisFounder.WorkingExperience = input.WorkingExperience;
            var rs = await _founderRepository.UpdateAsync(thisFounder);
            return ObjectMapper.Map<FSI.Domain.Founder.Founder, FounderDto>(rs);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _founderRepository.DeleteAsync(id);
        }
    }
}
