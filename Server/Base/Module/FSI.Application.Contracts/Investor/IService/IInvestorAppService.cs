using FSI.Application.Contracts.Investor.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Investor.IService
{
    public interface IInvestorAppService
    {
        Task<InvestorDto> InsertInvestorAsync(CreateInvestorDto input);

        Task<List<InvestorDto>> GetListAsync();

        Task<bool> GetCheckIsNewProfile();
    }
}
