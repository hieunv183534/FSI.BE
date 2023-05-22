using FSI.Domain.Startuper;
using FSI.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreStartuperRepository : EfCoreRepository<FSIDbContext, Startuper, Guid>, IStartuperRepository
    {
        public EfCoreStartuperRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
