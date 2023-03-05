using FSI.Application.Contracts.Test.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Test.IService
{
    public interface ITestAppService
    {
        Task<TestDto> InsertTestAsync(CreateTestDto input);

        Task<List<TestDto>> GetListAsync();

        Task DeleteAsync(Guid id);
    }
}
