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
    public class ProjectEvent : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Guid? PosterId { get; set; }

        public UserRoot? Poster { get; set; }

        public ProjectEventType Type { get; set; }

        public List<Guid>? FileIds { get; set; }

        public string? Content { get; set; }

        public List<string>? Images { get; set; }

        public string? Location { get; set; }

        public Guid? UserId { get; set; }

        public UserRoot? User { get; set; }

        public int? Invesment { get; set; }

        public DateTime? EventTime { get; set; }

        public ProjectStage? Stage { get; set; }

        public List<string>? Links { get; set; }
    }
}
