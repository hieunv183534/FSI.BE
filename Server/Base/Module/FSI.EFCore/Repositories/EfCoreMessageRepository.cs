using FSI.Domain.Chat;
using FSI.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreMessageRepository : EfCoreRepository<FSIDbContext, Message, Guid>, IMessageRepository
    {
        public EfCoreMessageRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
