using FSI.Application.Contracts.File;
using FSI.Domain.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectFileDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public ProjectDto Project { get; set; }

        public Guid FileId { get; set; }

        public FileInfomationDto File { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public bool VisibleForInvestor { get; set; }

        public bool VisibleForAll { get; set; }
    }
}
