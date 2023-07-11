using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Admin.DTO
{
    public class GetListProjectForAdminDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public List<ProjectStage> Stages { get; set; }

        public List<int> Fields { get; set; }

        public List<int> Areas { get; set; }

        public bool? IsActive { get; set; }
    }
}
