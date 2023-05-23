using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Investor.DTO
{
    public class CreateInvestorDto
    {
        public string InvestorName { get; set; }

        public int MinInvestValue { get; set; }

        public int MaxInvestValue { get; set; }

        public string BasicDescription { get; set; }

        public List<string> InvestFields { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }
    }
}
