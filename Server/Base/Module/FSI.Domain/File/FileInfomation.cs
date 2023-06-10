using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.File
{
    public class FileInfomation : FullAuditedAggregateRoot<Guid>
    {
        public Guid AuthorId { get; set; }

        public UserRoot Author { get; set; }

        public string Url { get; set; }

        public long Size { get; set; }

    }
}
