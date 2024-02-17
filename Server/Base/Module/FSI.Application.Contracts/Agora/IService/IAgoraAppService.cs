using FSI.Application.Contracts.Agora.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Agora.IService
{
    public interface IAgoraAppService
    {
        Task<string> CreateRtcToken(GetTokenDto input);

        Task<string> LoginAsGuestToMeet(GuestToMeetDto input);
    }
}
