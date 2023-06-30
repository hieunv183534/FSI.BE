using FSI.Application.Contracts.Investor.DTO;
using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Startuper.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.User.DTO
{
    public class UserDetailDto
    {
        public StartuperDto StartuperInfo { get; set; }

        public InvestorDto InvestorInfo { get; set; }

        public List<ProjectUserDto> ProjectAsStartuper { get; set; }

        public List<ProjectUserDto> ProjectAsInvestor { get; set; }
    }
}
