using Grpc.Net.Client;
using GrpcGreeterClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FSI.GrpcClient.RecommendationSystem
{
    public class RecommendationSystem : IRecommendationSystem
    {
        public async Task<string> Test(string name)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:7000");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                  new HelloRequestHieuNV { Name = name });
            return "OKOKOK " + reply.Message;
        }
    }
}
