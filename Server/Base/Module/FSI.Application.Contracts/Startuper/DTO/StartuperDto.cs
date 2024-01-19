using FSI.Application.Contracts.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Startuper.DTO
{
    public class StartuperDto : UserRootDto
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

        public List<string>? Collab { get; set; }

        public List<int>? RequestPersonality { get; set; }

        public List<int>? RequestSkill { get; set; }

        public bool? hasIdea { get; set; }

        public int Purpose { get; set; }

        public List<int>? ideaField { get; set; }

        public List<int>? targetField { get; set; }

        public List<int>? Specialize { get; set; }

        public List<int>? targetSpecialize { get; set; }

    }
}
