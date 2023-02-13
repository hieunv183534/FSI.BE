using FSI.Domain.Test;
using Microsoft.AspNetCore.SignalR;

namespace FSI.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITestRepository _testRepository;

        public ChatHub(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task AddTest(string name)
        {
            var rs = await _testRepository.InsertAsync(new Test()
            {
                Name = name,
                Code = name,
                Description = name
            });

            await Clients.All.SendAsync("OnCreatedTest", rs);
        }
    }
}
