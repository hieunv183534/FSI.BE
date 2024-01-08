using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.DTO
{
    public class UniversitySpecialized
    {
        public UniversitySpecialized(string universityName, List<string> specializeds)
        {
            UniversityName = universityName;
            Specializeds = specializeds;
        }

        public string UniversityName { get; set; }

        public List<string> Specializeds { get; set; }
    }
}
