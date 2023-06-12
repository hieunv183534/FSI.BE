using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FSI.Domain.Project
{
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        Task<List<Project>> GetListProjectForStartuper(string? filter, int? area, int? field, ProjectStage? stage, int? availableTime);
    }
}
