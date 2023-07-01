using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Project.DTO
{
    public class GetProjectEventsDto : PagedAndSortedResultRequestDto
    {
        public Guid ProjectId { get; set; }

        public string? Filter { get; set; }

        public int Type { get; set; }
    }
}
