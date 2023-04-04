using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.GrpcClient.RecommendationSystem
{
    public interface IRecommendationSystem
    {
        Task<string> Test(string name);
    }
}
