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
    public class Project : FullAuditedAggregateRoot<Guid>
    {
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public List<string> Fields { get; set; }

        public ProjectStage Stage { get; set; }

        public DateTime FoundedTime { get; set; }

        public string? Area { get; set; }

        public string? Website { get; set; }

        public string? Fb { get; set; }

        public string? Compliment { get; set; }

        public List<ProjectHistoryEvent> History { get; set; }

        public string? AvatarUrl { get; set; }

        public Guid FounderId { get; set; }

        public UserRoot Founder { get; set; }

    }
}
