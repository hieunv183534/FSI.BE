using FSI.Application.Contracts.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Auth.DTO
{
    public class RegisterDto
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string RoleRegister { get; set; }

        public UserRootDto BaseInfomation { get; set; }
    }
}
