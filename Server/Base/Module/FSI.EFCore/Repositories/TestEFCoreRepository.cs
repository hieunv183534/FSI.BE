using FSI.Domain.Test;
using FSI.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class TestEFCoreRepository : EfCoreRepository<FSIDbContext, Test, Guid>, ITestRepository
    {
        public TestEFCoreRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
