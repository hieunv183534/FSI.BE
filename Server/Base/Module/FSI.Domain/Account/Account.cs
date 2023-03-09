using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Account
{
    public class Account : FullAuditedAggregateRoot<Guid>
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }
    }
}
