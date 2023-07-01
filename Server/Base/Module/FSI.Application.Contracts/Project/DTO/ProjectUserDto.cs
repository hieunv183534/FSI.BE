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
    public class ProjectUserDto : FullAuditedAggregateRoot<Guid>
    {
        public ProjectDto Project { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public UserRootDto User { get; set; }

        public RoleInProject Role { get; set; }

        public bool IsActive { get; set; }

        public int TotalExpectedInvestment { get; set; }

        public int TotalInvestment { get; set; }

        public DateTime? JoinTime { get; set; }
    }
}
