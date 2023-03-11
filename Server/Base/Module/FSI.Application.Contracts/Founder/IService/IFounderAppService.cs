using FSI.Application.Contracts.Founder.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Founder.IService
{
    public interface IFounderAppService
    {
        Task<FounderDto> InsertFounderAsync(CreateFounderDto input);

        Task<List<FounderDto>> GetListAsync();
    }
}
