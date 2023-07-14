using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Startuper
{
    public class StartuperSimilarity : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }

        public Guid TargetId { get; set; }

        public float Similarity { get; set; }
    }
}
