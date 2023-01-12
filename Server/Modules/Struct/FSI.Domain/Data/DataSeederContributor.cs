using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace FSI
{
    public class DataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;

        private readonly IGuidGenerator _guidGenerator;
        public DataSeederContributor(
            IGuidGenerator guidGenerator,IObjectMapper objectMapper )
        {

            _guidGenerator = guidGenerator;
            _objectMapper = objectMapper;
        }

        public async Task SeedAsync(DataSeedContext context)
        {

        }
    }
}
