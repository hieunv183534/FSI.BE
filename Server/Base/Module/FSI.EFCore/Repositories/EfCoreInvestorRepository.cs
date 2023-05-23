using FSI.Domain.Investor;
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
    public class EfCoreInvestorRepository : EfCoreRepository<FSIDbContext, Investor, Guid>, IInvestorRepository
    {
        public EfCoreInvestorRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
