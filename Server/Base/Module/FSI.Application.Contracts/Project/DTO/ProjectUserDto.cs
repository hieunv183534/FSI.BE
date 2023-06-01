using FSI.Common.Enums;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectUserDto
    {
        public ProjectDto Project { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public RoleInProject Role { get; set; }

        public bool IsActive { get; set; }

        public int TotalExpectedInvestment { get; set; }

        public int TotalInvestment { get; set; }
    }
}
