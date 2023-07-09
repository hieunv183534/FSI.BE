using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectWorkDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public WorkStatus Status { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public Guid? AssignorId { get; set; }

        public UserRootDto? Assignor { get; set; }

        public Guid? AssigneeId { get; set; }

        public UserRootDto? Assignee { get; set; }

        public DateTime? Deadline { get; set; }

        public List<Guid>? FileIds { get; set; }
    }
}
