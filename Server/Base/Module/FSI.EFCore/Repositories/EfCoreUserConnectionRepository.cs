using FSI.Domain.User;
using FSI.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreUserConnectionRepository : EfCoreRepository<FSIDbContext, UserConnection, Guid>, IUserConnectionRepository
    {
        public EfCoreUserConnectionRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
