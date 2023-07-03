using FSI.Application.Contracts.User.DTO;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectCalendarEventDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Guid CreatedById { get; set; }

        public UserRootDto CreatedBy { get; set; }

        public int Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool AllDay { get; set; }

        public bool AutoDeleteWhenEnd { get; set; }
    }
}
