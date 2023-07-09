using FSI.Application.Contracts.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class AddProjectWorkDto
    {
        public Guid ProjectId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public Guid? AssigneeId { get; set; }

        public DateTime? Deadline { get; set; }

        public List<Guid>? FileIds { get; set; }
    }
}
