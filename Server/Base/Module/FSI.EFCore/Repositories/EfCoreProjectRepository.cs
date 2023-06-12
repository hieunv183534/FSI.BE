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

        public async Task<List<Project>> GetListProjectForStartuper(string? filter, int? area, int? field, ProjectStage? stage, int? availableTime)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.WhereIf(!String.IsNullOrWhiteSpace(filter), x => x.ProjectName.Contains(filter))
                .WhereIf(area.HasValue, x => x.Area == area)
                .WhereIf(stage.HasValue, x => x.Stage == stage)
                .ToListAsync();
        }
    }
}
