using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Startuper.DTO
{
    public class GetListStartuperForStartuperDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }

        public List<int>? Fields { get; set; }

        public List<int>? Personalities { get; set; }

        public List<int>? Skills { get; set; }

        public List<int> AvailableTimes { get; set; }

        public List<int> YearOfExps { get; set; }
            
        public List<int> Areas { get; set; }

        public Guid? Mode { get; set; }

        public bool? IsStudent { get; set; }

        public string? University { get; set; }

        public string? UniversitySpecialized { get; set; }

        public string? StudentId { get; set; }
    }
}
