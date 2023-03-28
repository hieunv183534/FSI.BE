using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class Project : FullAuditedAggregateRoot<Guid>
    {
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string Field { get; set; }

        public ProjectStage Stage { get; set; }

    }
}
