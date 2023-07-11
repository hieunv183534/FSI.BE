using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Admin.DTO
{
    public class RegisterAdminDto
    {
        public string Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
    }
}
