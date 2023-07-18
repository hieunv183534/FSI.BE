using FSI;
using FSI.Application.Contracts.Auth.IService;
using FSI.Application.EventHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FSI.Samples
{
    public class AuthAppSeviceTests : FSIApplicationTestBase
    {
        [Fact]
        public async Task Login_Should_Ok()
        {
            var similar = SimilarityUtil.CalculateCosineSimilarity(new float[] { 1,0 }, new float[] { 0,1 });

            Assert.Equal(0, similar);
        }
    }
}
