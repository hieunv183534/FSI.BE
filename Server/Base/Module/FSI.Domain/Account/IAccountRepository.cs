using FSI.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FSI.Domain.Account
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
    }
}
