using FSI.Domain.Chat;
using FSI.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreConversationRepository : EfCoreRepository<FSIDbContext, Conversation, Guid>, IConversationRepository
    {
        public EfCoreConversationRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
