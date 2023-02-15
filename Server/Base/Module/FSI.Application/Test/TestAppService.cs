using FSI.Application.Contracts.Test.DTO;
using FSI.Application.Contracts.Test.IService;
using FSI.Domain.Test;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Test
{
    [Authorize]
    //[RemoteService(false)]
    public class TestAppService : ApplicationService, ITestAppService
    {
        private readonly ITestRepository _testRepository;

        public TestAppService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<List<TestDto>> GetListAsync()
        {
            var rs = await _testRepository.GetListAsync();
            return ObjectMapper.Map< List<FSI.Domain.Test.Test>, List<TestDto>>(rs);
        }

        public async Task<TestDto> InsertTestAsync(CreateTestDto input)
        {
            var rs = await _testRepository.InsertAsync(ObjectMapper.Map<CreateTestDto, FSI.Domain.Test.Test>(input));
            return ObjectMapper.Map<FSI.Domain.Test.Test, TestDto>(rs);
        }
    }
}
