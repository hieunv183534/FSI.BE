using FSI.Domain.Founder;
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
    public class EfCoreFounderRepository : EfCoreRepository<FSIDbContext, Founder, Guid>, IFounderRepository
    {
        public EfCoreFounderRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
