using FSI.Application.Contracts.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Founder.DTO
{
    public class FounderDto : UserRootDto
    {
        public string Speciality { get; set; }

        public string Personality { get; set; }

        public string Skill { get; set; }

        public string WorkingExperience { get; set; }

        public string Activity { get; set; }

        public string Certificate { get; set; }

        public string Award { get; set; }

        public string FavoriteField { get; set; }

        public bool hasProject { get; set; }
    }
}
