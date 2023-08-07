using FSI.Application.Contracts.Project.DTO;
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

        Task<PagedResultDto<StartuperDto>> PostToGetListStartuper(GetListStartuperForStartuperDto input);

        Task<bool> GetCheckIsNewProfile();

        Task UploadAvatar();

        Task<StartuperDto> GetMyInfoAsync();

        Task PostUpdateBaseInfo(UpdateBaseInfoDto input);

        Task<List<ProjectUserDto>> GetMyProjects();

        Task RequestFriendToOrtherStartuper(Guid targetId);

        Task AcceptRequestFriendFromOrtherStartuper(Guid targetId);

        Task CancelRequestToOrtherStartuper(Guid targetId);

        Task<UserDetailDto> GetUserDetail(Guid userId);

        Task<UserRootDto> GetUserByUsername(string username);
    }
}
