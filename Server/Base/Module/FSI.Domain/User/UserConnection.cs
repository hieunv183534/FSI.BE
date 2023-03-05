using FSI.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.User
{
    public class UserConnection : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        public UserRoot User { get; set; }

        public string ConnectionId { get; set; }

    }
}
