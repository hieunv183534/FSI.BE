using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.User
{
    public class UserRoot : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string IdentityCard { get; set; }

        public string Location { get; set; }

        public string WorkingPlace { get; set; }

    }
}
