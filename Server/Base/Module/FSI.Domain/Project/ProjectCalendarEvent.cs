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
    public class ProjectCalendarEvent : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid CreatedById { get; set; }

        public UserRoot CreatedBy { get; set; }

        public CalendarEventType Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool AllDay { get; set; }

        public bool AutoDeleteWhenEnd { get; set; }

        public string? Title { get; set; }

        public bool IsPublic { get; set; }
    }
}
