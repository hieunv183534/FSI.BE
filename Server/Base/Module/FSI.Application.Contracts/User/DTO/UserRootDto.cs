using FSI.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.User.DTO
{
    public class UserRootDto :  AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string IdentityCard { get; set; }

        public int? Location { get; set; }

        public string WorkingPlace { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
