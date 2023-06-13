using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectHistoryEventDto
    {
        public ProjectStage Stage { get; set; }

        public ProjectEventType Type { get; set; }

        public string Detail { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
