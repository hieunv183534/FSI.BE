using FSI.Domain.User;
using FSI.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreUserRootRepository : EfCoreRepository<FSIDbContext, UserRoot, Guid>, IUserRootRepository
    {
        public EfCoreUserRootRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
