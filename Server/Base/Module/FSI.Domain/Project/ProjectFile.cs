using FSI.Domain.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Project
{
    public class ProjectFile : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid FileId { get; set; }

        public FileInfomation File { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public bool VisibleForInvestor { get; set; } = false;

        public bool VisibleForAll { get; set; } = false;
    }
}
