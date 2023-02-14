using FSI.Application.Contracts.Test.DTO;
using FSI.Domain.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
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

        public async Task AddTest(CreateTestDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var rs = await _testRepository.InsertAsync(_objectMapper.Map<CreateTestDto, FSI.Domain.Test.Test>(input));
                await uow.CompleteAsync();
                await Clients.All.SendAsync("OnCreatedTest", await _testRepository.GetListAsync());
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
