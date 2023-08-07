using FSI.Application.Contracts.User.DTO;
using FSI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class AddProjectCalendarEventDto
    {
        public Guid ProjectId { get; set; }

        public CalendarEventType Type { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool AllDay { get; set; }

        public bool AutoDeleteWhenEnd { get; set; }

        public string? Title { get; set; }

        public bool IsPublic { get; set; }
    }
}
