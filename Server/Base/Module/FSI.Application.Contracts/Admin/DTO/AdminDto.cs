using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Admin.DTO
{
    public class AdminDto : FullAuditedAggregateRoot<Guid>
    {
        public string Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
