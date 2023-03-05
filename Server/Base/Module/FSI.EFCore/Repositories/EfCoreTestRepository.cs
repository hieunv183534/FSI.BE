using FSI.Domain.Test;
using FSI.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreTestRepository : EfCoreRepository<FSIDbContext, Test, Guid>, ITestRepository
    {
        public EfCoreTestRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
