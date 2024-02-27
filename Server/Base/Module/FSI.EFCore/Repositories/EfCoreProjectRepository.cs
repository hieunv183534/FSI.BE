using FSI.Common.Enums;
using FSI.Domain.Investor;
using FSI.Domain.Project;
using FSI.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace FSI.EFCore.Repositories
{
    public class EfCoreProjectRepository : EfCoreRepository<FSIDbContext, Project, Guid>, IProjectRepository
    {
        public EfCoreProjectRepository(IDbContextProvider<FSIDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Project> GetProjectWithHirings(Guid projectId)
        {
            var dbSet = await GetDbSetAsync();
            var project = dbSet.Include("Hirings").FirstOrDefault(x=> x.Id == projectId);
            return project;
        }
    }
}
