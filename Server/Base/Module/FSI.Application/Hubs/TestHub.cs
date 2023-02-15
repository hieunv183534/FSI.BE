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
using Volo.Abp.Uow;

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

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task AddTest(CreateTestDto input)
        {
            var test = _objectMapper.Map<CreateTestDto, Domain.Test.Test>(input);
            var rs = await _testRepository.InsertAsync(test);
            await Clients.All.SendAsync("OnCreatedTest", await _testRepository.GetCountAsync());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
