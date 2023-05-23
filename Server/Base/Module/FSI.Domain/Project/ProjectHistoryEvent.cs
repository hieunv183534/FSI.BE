using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class ProjectHistoryEvent : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public ProjectStage Stage { get; set; }

        public ProjectEventType Type { get; set; }

        public string Detail { get; set; }
    }
}
