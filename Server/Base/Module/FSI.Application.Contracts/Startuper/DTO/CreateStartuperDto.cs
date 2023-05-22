using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Startuper.DTO
{
    public class CreateStartuperDto
    {
        public string Speciality { get; set; }

        public string Personality { get; set; }

        public string Skill { get; set; }

        public string WorkingExperience { get; set; }

        public string Activity { get; set; }

        public string Certificate { get; set; }

        public string Award { get; set; }

        public string FavoriteField { get; set; }
    }
}
