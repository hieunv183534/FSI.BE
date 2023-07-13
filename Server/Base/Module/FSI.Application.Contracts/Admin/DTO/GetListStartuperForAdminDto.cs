using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Admin.DTO
{
    public class GetListStartuperForAdminDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public List<int>? Fields { get; set; }

        public List<int> Areas { get; set; }
    }
}
