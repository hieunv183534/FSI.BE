using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Project.IService;
using FSI.Domain.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace FSI.Application.Project
{
    public class ProjectAppService : ApplicationService, IProjectAppService
    {
        private readonly IProjectRepository _projectRepository;


        public ProjectAppService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> InsertProjectAsync(CreateProjectDto input)
        {
            var project = await _projectRepository.InsertAsync(new Domain.Project.Project()
            {
                Area= input.Area,
                AvatarUrl= input.AvatarUrl,
                Compliment =  input.Compliment,
                Description= input.Description,
                Fb = input.Fb,
                Fields= input.Fields,
                History= input.History, 
                Stage= input.Stage,
                Website= input.Website,
                FoundedTime= input.FoundedTime, 
                ProjectName= input.ProjectName
            });

            throw new NotImplementedException();
        }
    }
}
