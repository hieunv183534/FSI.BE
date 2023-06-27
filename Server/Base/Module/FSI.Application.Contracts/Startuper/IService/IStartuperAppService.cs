using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FSI.Application.Contracts.Startuper.IService
{
    public interface IStartuperAppService
    {
        Task<StartuperDto> InsertStartuperAsync(CreateStartuperDto input);

        Task<List<StartuperDto>> GetListAsync();

        Task<PagedResultDto<StartuperDto>> PostToGetListStartuper(GetListStartuperForProjectDto input);

        Task<PagedResultDto<StartuperDto>> GetListFounder(GetListFounderDto input);

        Task<bool> GetCheckIsNewProfile();

        Task UploadAvatar();

        Task<StartuperDto> GetMyInfoAsync();

        Task UpdateBaseInfo(UpdateBaseInfoDto input);
    }
}
