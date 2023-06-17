using FSI.Application.Contracts.User.DTO;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.File
{
    public class FileInfomationDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid AuthorId { get; set; }

        public UserRootDto Author { get; set; }

        public string Url { get; set; }

        public long Size { get; set; }

        public string ContentType { get; set; }
    }
}
