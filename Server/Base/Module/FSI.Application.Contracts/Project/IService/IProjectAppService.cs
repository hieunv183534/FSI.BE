using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.IService
{
    public interface IProjectAppService
    {
        Task<ProjectDto> InsertProjectAsync(CreateProjectDto input);
    }
}
