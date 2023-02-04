using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace FSI.Data
{
    public class GeneralCategoryDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        //private readonly IRepository<GeneralCategory, Guid> _categoryRepository;IRepository<GeneralCategory, Guid> categoryRepository,
        private readonly IGuidGenerator _guidGenerator;

        public GeneralCategoryDataSeederContributor( IGuidGenerator guidGenerator)
        {
            //_categoryRepository = categoryRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
        }
    }
}
