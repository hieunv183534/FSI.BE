using Volo.Abp.Domain.Repositories;

namespace FSI.Domain.User
{
    public interface IUserRootRepository : IRepository<UserRoot, Guid>
    {
    }
}
