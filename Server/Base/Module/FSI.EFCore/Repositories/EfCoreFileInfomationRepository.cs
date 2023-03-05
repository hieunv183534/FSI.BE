using FSI.Domain.File;
using FSI.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreFileInfomationRepository : EfCoreRepository<FSIDbContext, FileInfomation, Guid>, IFileInfomationRepository
    {
        public EfCoreFileInfomationRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
