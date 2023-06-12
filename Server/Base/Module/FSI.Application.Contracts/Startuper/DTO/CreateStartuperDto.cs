using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Startuper.DTO
{
    public class CreateStartuperDto
    {
        public int? Field { get; set; }

        public string? Speciality { get; set; }

        public List<int>? Personality { get; set; }

        public List<int>? Skill { get; set; }

        public string? WorkingExperience { get; set; }

        public string? Activity { get; set; }

        public string? CertificateAndAward { get; set; }

        public bool? hasProject { get; set; }

        public string? Describe { get; set; }

        public int? YearOfExp { get; set; }

        public int? AvailableTime { get; set; }
    }
}
