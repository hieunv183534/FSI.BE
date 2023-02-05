using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Test
{
    public class Test : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
