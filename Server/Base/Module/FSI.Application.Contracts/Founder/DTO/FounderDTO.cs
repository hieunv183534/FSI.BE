using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Founder.DTO
{
    public class FounderDto : ExtensibleAuditedEntityDto<Guid>
    {
        public enum Speciality { low, medium, high }

        public enum Personality { low, medium, high }

        public enum Skill { low, medium, high }

        public string workingExperience { get; set; }

        public string Activity { get; set; }

        public string Certificate { get; set; }

        public string Award { get; set; }

        public enum favoriteField { low, medium, high }

        public Boolean hasProject;
    }
}
