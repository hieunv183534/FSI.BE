using FSI.Application.Contracts.Founder.DTO;
using FSI.Application.Contracts.Founder.IService;
using FSI.Application.Hubs;
using FSI.Domain.Founder;
using FSI.Domain.Test;
using FSI.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Founder
{
    [Authorize]
    //[RemoteService(false)]
    [Authorize(Roles = "Founder")]
    public class FounderAppService : ApplicationService, IFounderAppService
    {
        private readonly IFounderRepository _founderRepository;
        public FounderAppService(IFounderRepository founderRepository)
        {
            _founderRepository = founderRepository;
        }

        public async Task<List<FounderDto>> GetListAsync()
        {
            var rs = await _founderRepository.GetListAsync();
            return ObjectMapper.Map<List<FSI.Domain.Founder.Founder>, List<FounderDto>>(rs);
        }

        public async Task<FounderDto> InsertFounderAsync(CreateFounderDto input)
        {
            var rs = await _founderRepository.InsertAsync(ObjectMapper.Map<CreateFounderDto, FSI.Domain.Founder.Founder>(input));
            return ObjectMapper.Map<FSI.Domain.Founder.Founder, FounderDto>(rs);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _founderRepository.DeleteAsync(id);
        }
    }
}
