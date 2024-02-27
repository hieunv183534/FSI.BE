using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Project.DTO.Hiring
{
    public class ProjectHiringDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }

        public int Quantity { get; set; }

        public int Specialize { get; set; }

        public WorkingForm WorkingForm { get; set; }

        public int? Location { get; set; }

        public string? WorkingAddress { get; set; }

        public List<int>? WorkingTimes { get; set; }

        public string? Description { get; set; }

        public List<int>? YearOfExps { get; set; }

        public int? Degree { get; set; }

        public List<int>? Skills { get; set; }

        public List<int>? Personalities { get; set; }

        public string? OtherRequest { get; set; }

        public string? OtherDetail { get; set; }

        public DateTime? Duration { get; set; }


        public int IncomeMode { get; set; }

        public int? IncomeFrom { get; set; }

        public int? IncomeTo { get; set; }

        public int? IncomeRange { get; set; }
    }
}
