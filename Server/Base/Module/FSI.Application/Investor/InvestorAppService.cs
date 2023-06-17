using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Investor.IService;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Startuper.IService;
using FSI.Domain.Investor;
using FSI.Domain.Startuper;
using FSI.GrpcClient.RecommendationSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Investor
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class InvestorAppService : ApplicationService, IInvestorAppService
    {
        private readonly IInvestorRepository _investorRepository;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid currentUserId;

        private readonly IRecommendationSystem _recommendationSystem;

        public InvestorAppService(IInvestorRepository investorRepository, IHttpContextAccessor httpContextAccessor, IRecommendationSystem recommendationSystem)
        {
            _investorRepository = investorRepository;
            _httpContextAccessor = httpContextAccessor;
            this.currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _recommendationSystem = recommendationSystem;
        }
        public async Task<List<InvestorDto>> GetListAsync()
        {
            var rs = await _investorRepository.GetListAsync();
            return ObjectMapper.Map<List<FSI.Domain.Investor.Investor>,List<InvestorDto>>(rs);
        }

        public async Task<InvestorDto> InsertInvestorAsync(CreateInvestorDto input)
        {
            var thisInvestor = await _investorRepository.GetAsync(this.currentUserId);
            thisInvestor.InvestorName = input.InvestorName;
            thisInvestor.InvestFields = input.InvestFields;
            thisInvestor.MinInvestValue = input.MinInvestValue;
            thisInvestor.MaxInvestValue = input.MaxInvestValue;
            thisInvestor.BasicDescription = input.BasicDescription;
            thisInvestor.Company = input.Company;
            thisInvestor.Position = input.Position;

            var rs = await _investorRepository.UpdateAsync(thisInvestor);
            return ObjectMapper.Map<FSI.Domain.Investor.Investor, InvestorDto>(rs);
        }

        public async Task<bool> GetCheckIsNewProfile()
        {
            var investor = await _investorRepository.GetAsync(this.currentUserId);

            return (bool)investor.IsNewProfile;
        }
    }
}
