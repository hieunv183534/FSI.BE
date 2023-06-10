using FSI.Common.Enums;
using FSI.Domain.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class CreateProjectDto
    {
        public Guid? ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public string? Description { get; set; }

        public List<int>? Fields { get; set; }

        public ProjectStage? Stage { get; set; }

        public DateTime? FoundedTime { get; set; }

        public string? Area { get; set; }

        public string? Website { get; set; }

        public string? Fb { get; set; }

        public string? Compliment { get; set; }

        public List<ProjectHistoryEvent>? History { get; set; }

        public string AvatarUrl { get; set; }
    }
}
