using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Test.DTO
{
    public class TestDto : ExtensibleAuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
