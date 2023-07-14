using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class ProjectSimilarity : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Guid ProjectTargetId { get; set; }

        public float Similarity { get; set; }
    }
}
