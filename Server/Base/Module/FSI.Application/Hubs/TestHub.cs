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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public TestHub(ITestRepository testRepository, IObjectMapper objectMapper, IUnitOfWorkManager unitOfWorkManager)
        {
            _testRepository = testRepository;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task AddTest(CreateTestDto input)
        {
            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var test = _objectMapper.Map<CreateTestDto, Domain.Test.Test>(input);
                var rs = await _testRepository.InsertAsync(test);
                await uow.CompleteAsync();
            }

            using (var uow = _unitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                var rs = await _testRepository.GetListAsync();
                await uow.CompleteAsync();
                await Clients.All.SendAsync("OnCreatedTest", rs);
            }
            
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
