using FSI.Common.Enums;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class ProjectWork : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public WorkStatus Status { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public Guid? AssignorId { get; set; }

        public UserRoot? Assignor { get; set; }

        public Guid? AssigneeId { get; set; }

        public UserRoot? Assignee { get; set; }

        public DateTime? Deadline { get; set; }

        public List<Guid>? FileIds { get; set; }
    }
}
