using FSI.Application.Contracts.Auth.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Auth.IService
{
    public interface IAuthAppService
    {
        string Login(LoginDto loginDto);
    }
}
