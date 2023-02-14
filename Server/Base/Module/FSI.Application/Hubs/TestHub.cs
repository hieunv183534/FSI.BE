using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.Hubs
{
    [Authorize]
    public class TestHub : Hub
    {
        private readonly ITestRepository _testRepository;
        private readonly IObjectMapper _objectMapper;

        public TestHub(ITestRepository testRepository, IObjectMapper objectMapper)
        {
            _testRepository = testRepository;
            _objectMapper = objectMapper;
        }

        public async Task AddTest(CreateTestDto input)
        {
            var rs = await _testRepository.InsertAsync(_objectMapper.Map<CreateTestDto, FSI.Domain.Test.Test>(input));

            await Clients.All.SendAsync("OnCreatedTest", await _testRepository.GetListAsync());

            Task.CompletedTask.Wait();
        }
    }
}
