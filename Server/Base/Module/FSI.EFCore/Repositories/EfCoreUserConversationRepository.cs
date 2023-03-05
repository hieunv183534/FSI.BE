using FSI.Domain.Chat;
using FSI.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreUserConversationRepository : EfCoreRepository<FSIDbContext, UserConversation, Guid>, IUserConversationRepository
    {
        public EfCoreUserConversationRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
