using FSI.Domain.User;
using FSI.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreFounderRepository : EfCoreRepository<FSIDbContext, Founder, Guid>, IFounderRepository
    {
        public EfCoreFounderRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
