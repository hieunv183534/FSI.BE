using FSI.Application.Contracts.Project.DTO.Hiring;
using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using FSI.Domain.Project;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectDto : FullAuditedAggregateRoot<Guid>
    {
        public string ProjectName { get; set; }

        public string? Description { get; set; }

        public List<int> Fields { get; set; }

        public ProjectStage Stage { get; set; }

        public DateTime FoundedTime { get; set; }

        public int Area { get; set; }

        public string? Website { get; set; }

        public string? Fb { get; set; }

        public string? Compliment { get; set; }

        public string? AvatarUrl { get; set; }

        public Guid FounderId { get; set; }

        public UserRootDto? Founder { get; set; }

        public bool? IsActive { get; set; }

        public List<ProjectHiringDto>? Hirings { get; set; }

        public int Scale { get; set; }

        public bool IsProfit { get; set; }
    }
}
