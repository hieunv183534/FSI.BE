using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.User
{
    public class Friend : FullAuditedAggregateRoot<Guid>
    {
        public UserRoot UserA { get; set; }

        public Guid UserAId { get; set; }

        public UserRoot UserB { get; set;}

        public Guid UserBId { get; set; }

        public bool IsActive { get; set; }
    }
}
