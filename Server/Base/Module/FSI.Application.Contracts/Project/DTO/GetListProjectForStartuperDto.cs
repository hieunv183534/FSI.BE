using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Project.DTO
{
    public class GetListProjectForStartuperDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public ProjectStage? Stage { get; set; }

        public int? Field { get; set; }

        public int? Area { get; set; }

        public int? AvailableTime { get; set; }
    }
}
