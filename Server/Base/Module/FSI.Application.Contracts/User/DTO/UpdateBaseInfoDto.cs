using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.User.DTO
{
    public class UpdateBaseInfoDto
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Location { get; set; }

        public string? WorkingPlace { get; set; }

        public int Job { get; set; }

        public bool Gender { get; set; }

        public string? University { get; set; }

        public string? UniversitySpecialized { get; set; }

        public string? StudentId { get; set; }
    }
}
