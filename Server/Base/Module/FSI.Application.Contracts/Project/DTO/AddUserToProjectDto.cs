using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class AddUserToProjectDto
    {
        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public RoleInProject Role { get; set; }
    }
}
