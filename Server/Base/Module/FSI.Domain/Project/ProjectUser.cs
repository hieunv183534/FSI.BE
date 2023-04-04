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
    public class ProjectUser : FullAuditedAggregateRoot<Guid>
    {
        public Project Project { get; set; }

        public Guid ProjectId { get; set; }

        public UserRoot User { get; set; }

        public Guid UserId { get; set; }

        public RoleInProject Role { get; set; }

        public bool IsActive { get; set; }
    }
}
