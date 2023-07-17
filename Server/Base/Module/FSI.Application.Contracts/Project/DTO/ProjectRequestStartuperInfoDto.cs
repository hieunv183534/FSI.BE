using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Application.Contracts.Project.DTO
{
    public class ProjectRequestStartuperInfoDto
    {
        public Guid ProjectId { get; set; }

        public string? EngText { get; set; }

        public List<int>? Locations { get; set; }

        public List<int>? Jobs { get; set; }

        public string? WorkingPlace { get; set; }

        public string? Describe { get; set; }

        public List<int>? Fields { get; set; }

        public string? Speciality { get; set; }

        public List<int>? Personalities { get; set; }

        public List<int>? Skills { get; set; }

        public string? WorkingExperience { get; set; }

        public string? Activity { get; set; }

        public string? CertificateAndAward { get; set; }

        public List<int>? YearOfExps { get; set; }

        public List<int>? AvailableTimes { get; set; }
    }
}
