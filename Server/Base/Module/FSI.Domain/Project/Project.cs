using FSI.Common.Enums;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class Project : FullAuditedAggregateRoot<Guid>
    {
        public string? ProjectName { get; set; }

        public string? Description { get; set; }

        public List<int>? Fields { get; set; }

        public ProjectStage? Stage { get; set; }

        public DateTime? FoundedTime { get; set; }

        public int? Area { get; set; }

        public string? Website { get; set; }

        public string? Fb { get; set; }

        public string? Compliment { get; set; }

        public string? AvatarUrl { get; set; }

        public Guid? FounderId { get; set; }

        public UserRoot? Founder { get; set; }

        public bool? IsHireNewMember { get; set; }

        public List<int>? AvailableTimeRequire { get; set; }

        public bool? IsActive { get; set; }

        public string? ProjectEnglishText { get; set; }

        public List<ProjectHiring>? Hirings { get; set; }

        public string TheLeanCanvasBusinessModel { get; set; }

    }

    public class ProjectHiring : Entity<Guid>
    {
        public string Title { get; set; }

        public int Quantity { get; set; }

        public int Specialize { get; set; }

        public WorkingForm WorkingForm { get; set; }

        public int? Location { get; set; }

        public string? WorkingAddress { get; set; }

        public List<int>? WorkingTimes { get; set; }

        public string? Income { get; set; }

        public string? Description { get; set; }

        public List<int>? YearOfExps { get; set; }

        public int? Degree { get; set; }

        public List<int>? Skills { get; set; }

        public List<int>? Personalities { get; set; }

        public string? OtherRequest { get; set; }

        public string? OtherDetail { get; set; }

        public DateTime? Duration { get; set; }
    }
}
